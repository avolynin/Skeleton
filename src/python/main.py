import nwpoints as nwp
from pipe import ClientPipe
from PIL import Image
import io

def imageToBytes(image):
    buf = io.BytesIO()
    image.save(buf, format='JPEG')
    
    return buf.getvalue()

def byteToImage(bytesImg):
    imageStream = io.BytesIO(bytesImg)
    imageFileJPG = Image.open(imageStream).convert("RGB")
    imageStream.close()

    return imageFileJPG;

pipe = ClientPipe(r'\\.\pipe\testpipe')

data = pipe.listen(75*1024)

test = byteToImage(data)
imgBones = nwp.getImageWithBones(test) 
bytesImg = imageToBytes(imgBones)

pipe.send(bytesImg)