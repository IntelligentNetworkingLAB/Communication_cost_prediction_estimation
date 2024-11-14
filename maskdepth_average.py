import argparse
import cv2
import glob
import matplotlib
import numpy as np
import os
import torch

from depth_anything_v2.dpt import DepthAnythingV2

def extract_depth_values(depth_image, mask_indices_path):
    # Load mask indices
    mask_indices = np.loadtxt(mask_indices_path, dtype=int)

    # Get image dimensions
    height, width = depth_image.shape

    # Create an empty mask
    mask = np.zeros((height * width), dtype=np.uint8)

    # Set mask indices to 1
    mask[mask_indices] = 1

    # Reshape mask to 2D
    mask = mask.reshape((height, width))

    # Extract depth values in the masked area
    masked_depth_values = depth_image[mask == 1]

    return masked_depth_values, mask

if __name__ == '__main__':
    parser = argparse.ArgumentParser(description='Depth Anything V2 with Mask Indices')
    
    parser.add_argument('--img-path', type=str, required=True)
    parser.add_argument('--mask-indices-path', type=str, required=True)
    parser.add_argument('--input-size', type=int, default=518)
    parser.add_argument('--outdir', type=str, default='./vis_depth')
    parser.add_argument('--encoder', type=str, default='vitl', choices=['vits', 'vitb', 'vitl', 'vitg'])
    parser.add_argument('--pred-only', dest='pred_only', action='store_true', help='only display the prediction')
    parser.add_argument('--grayscale', dest='grayscale', action='store_true', help='do not apply colorful palette')
    
    args = parser.parse_args()
    
    DEVICE = 'cuda' if torch.cuda.is_available() else 'mps' if torch.backends.mps.is_available() else 'cpu'
    
    model_configs = {
        'vits': {'encoder': 'vits', 'features': 64, 'out_channels': [48, 96, 192, 384]},
        'vitb': {'encoder': 'vitb', 'features': 128, 'out_channels': [96, 192, 384, 768]},
        'vitl': {'encoder': 'vitl', 'features': 256, 'out_channels': [256, 512, 1024, 1024]},
        'vitg': {'encoder': 'vitg', 'features': 384, 'out_channels': [1536, 1536, 1536, 1536]}
    }
    
    depth_anything = DepthAnythingV2(**model_configs[args.encoder])
    depth_anything.load_state_dict(torch.load(f'checkpoints/depth_anything_v2_{args.encoder}.pth', map_location='cpu'))
    depth_anything = depth_anything.to(DEVICE).eval()
    
    if os.path.isfile(args.img_path):
        if args.img_path.endswith('txt'):
            with open(args.img_path, 'r') as f:
                filenames = f.read().splitlines()
        else:
            filenames = [args.img_path]
    else:
        filenames = glob.glob(os.path.join(args.img_path, '**/*'), recursive=True)
    
    os.makedirs(args.outdir, exist_ok=True)
    
    cmap = matplotlib.colormaps.get_cmap('Spectral_r')
    
    for k, filename in enumerate(filenames):
        print(f'Progress {k+1}/{len(filenames)}: {filename}')
        
        raw_image = cv2.imread(filename)
        
        depth = depth_anything.infer_image(raw_image, args.input_size)
        
        depth = (depth - depth.min()) / (depth.max() - depth.min()) * 255.0
        depth = depth.astype(np.uint8)
        
        masked_depth_values, mask = extract_depth_values(depth, args.mask_indices_path)
        
        # Calculate the mean of the masked depth values
        mean_depth_value = np.mean(masked_depth_values)
        print(f'Mean depth value in the mask: {mean_depth_value}')

        output_path = os.path.join(args.outdir, os.path.splitext(os.path.basename(filename))[0] + '_masked_depth_values.txt')
        np.savetxt(output_path, masked_depth_values, fmt='%f')
        print(f'Masked depth values saved to {output_path}')

        # Apply mask to depth image
        masked_depth_image = np.zeros_like(depth)
        masked_depth_image[mask == 1] = depth[mask == 1]
        
        if args.grayscale:
            depth_colored = np.repeat(masked_depth_image[..., np.newaxis], 3, axis=-1)
        else:
            depth_colored = (cmap(masked_depth_image)[:, :, :3] * 255)[:, :, ::-1].astype(np.uint8)

        # Combine original image with depth image only in the masked areas
        combined_image = raw_image.copy()
        combined_image[mask == 1] = depth_colored[mask == 1]

        if not args.pred_only:
            split_region = np.ones((raw_image.shape[0], 50, 3), dtype=np.uint8) * 255
            combined_result = cv2.hconcat([raw_image, split_region, combined_image])
            cv2.imwrite(os.path.join(args.outdir, os.path.splitext(os.path.basename(filename))[0] + '.png'), combined_result)
        else:
            cv2.imwrite(os.path.join(args.outdir, os.path.splitext(os.path.basename(filename))[0] + '.png'), combined_image)

        print(f'Result saved to {os.path.join(args.outdir, os.path.splitext(os.path.basename(filename))[0] + ".png")}')
