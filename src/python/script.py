import os
import torchvision
from glob import glob

def createDir(path):
    if not os.path.exists(path):
        os.makedirs(path)
        
def main():
    dataset_path = r"..\dataset"
    dataset = glob(os.path.join(dataset_path, "*"))
    
    for data in dataset:
        image_path = glob(os.path.join(data, "*.jpg*"))
        json_path = glob(os.path.join(data, "*.json"))
        print(image_path)
        print(json_path)
        
if __name__ == '__main__':
    main()