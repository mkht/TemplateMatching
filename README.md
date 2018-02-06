TemplateMatching
====

Powershell cmdlet for template matching.  
You can find out where the template image is in the target image.

## Cmdlet
* Invoke-TemplateMatching

## Parameter
### Target
**_(Type = string, Mandatory = True, Positional = False)_**  
Specifies the image file in which you expect to find a match to the template image.

### Template
**_(Type = string, Mandatory = True, Positional = False)_**  
Specifies the patch image which will be compared to the target image.  
The image must be smaller than the target image.

### Threshold
**_(Type = double, Mandatory = False, Positional = False)_**  
Specifies similarity threshold for image matching.  
This cmdlet returns all matched points that has more similar than the threshold.  
You can specifies threshold between `0.1` to `1.0` (greater is best match)  
The default threshold is `0.95`

## Example
* Template image  
![Template](/Sample/template.png)

* Target image  
![Template](/Sample/target.png)

* Command
```PowerShell
PS C:\> Invoke-TemplateMatching -Target ".\target.png" -Template ".\template.png"

Rect                             Similarity
----                             ----------
(x:720 y:101 width:74 height:66)  0.9999997
```

The template image found in the target image.  
The coordinate of the image is `(x=720, y=101)`.  
Similarity is `0.9999997`, it's nearly equal `1`. that indicates complete match.

----
## License
TemplateMatching is released under the MIT License. See [LICENSE](/LICENSE).  

This module includes these libraries.  
* [OpenCV](https://opencv.org/)  
Licensed under the [BSD 3-Clause](https://opencv.org/license.html)

* [OpenCvSharp](https://github.com/shimat/opencvsharp)  
Licensed under the [BSD 3-Clause](https://github.com/shimat/opencvsharp/blob/master/LICENSE)