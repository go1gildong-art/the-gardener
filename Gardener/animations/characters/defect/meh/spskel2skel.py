from skimage import morphology
import numpy as np

file_path = "C:/Users/nines/Documents/sts2_moddev/the-gardener/Gardener/animations/characters/defect/meh/defect.skel.spskel"
data_type = np.uint8  # Change to np.float32, np.int32, etc. based on your data structure

# Read the binary file into a flat NumPy array
with open(file_path, "rb") as f:
    sparse_image = np.fromfile(f, dtype=data_type)

# Assuming 'sparse_image' is your binary input array representing the spskel
binary_image = sparse_image > 0

# Convert to standard 1-pixel wide skeleton (skel)
skel = morphology.skeletonize(binary_image)
