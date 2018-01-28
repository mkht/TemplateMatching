using OpenCvSharp;
using System;
using System.IO;
using System.Management.Automation;

namespace TemplateMatching
{
    /// <summary>
    /// This cmdlet search and find the location of a template image in a target image.
    /// </summary>
    [Cmdlet(VerbsLifecycle.Invoke, "TemplateMatching")]
    public class InvokeTemplateMatchingCommand : Cmdlet
    {
        #region Parameters
        /// <summary>
        /// This parameter specifies target image for template matching.
        /// </summary>
        [Parameter(Mandatory = true)]
        public string Target { get; set; }

        /// <summary>
        /// This parameter specifies template image for template matching.
        /// It must be not greater than the target image.
        /// </summary>
        [Parameter(Mandatory = true)]
        public string Template { get; set; }

        /// <summary>
        /// This parameter specifies similarity threshold of image matching.
        /// This cmdlet returns all matched points that has more similar than the threshold.
        /// You can specifies threshold between 0.1 to 1.0 (greater is best match)
        /// The default threshold is 0.95
        /// </summary>
        [Parameter()]
        [ValidateRange(0.1, 1.0)]
        public double Threshold { get; set; } = 0.95;
        #endregion Parameters

        #region Cmdlet Overrides
        protected override void ProcessRecord()
        {
            // File existence check
            if (!File.Exists(@Target))
            {
                WriteError(new ErrorRecord((new FileNotFoundException()), "FileNotFound", ErrorCategory.ObjectNotFound, Target));
                return;
            }
            if (!File.Exists(@Template))
            {
                WriteError(new ErrorRecord((new FileNotFoundException()), "FileNotFound", ErrorCategory.ObjectNotFound, Template));
                return;
            }

            try
            {
                //Load taraget and template image file
                using (var matTarget = new Mat(@Target))
                using (var matTemplate = new Mat(@Template))
                //Prepare result image
                using (var result = new Mat(matTarget.Height - matTemplate.Height + 1, matTarget.Width - matTemplate.Width + 1, MatType.CV_8UC1))
                {
                    var templateSize = new OpenCvSharp.Size(matTemplate.Width, matTemplate.Height);

                    do
                    {
                        // Invoke template matching
                        Cv2.MatchTemplate(matTarget, matTemplate, result, TemplateMatchModes.CCoeffNormed);
                        // Extract most similar point
                        Cv2.MinMaxLoc(result, out double minVal, out double maxVal, out Point minPoint, out Point maxPoint);

                        if (maxVal < Threshold)
                        {
                            break;
                        }
                        else
                        {
                            var matchedRect = new MatchedRect();
                            matchedRect.Rect = new Rect(maxPoint, templateSize);
                            matchedRect.Similarity = (float)maxVal;

                            // Return matched rectangle
                            WriteObject(matchedRect);

                            // Fill matched rectangle as random
                            Rect fillRect = matchedRect.Rect;
                            fillRect.Inflate(Convert.ToInt32(fillRect.Width * -0.2), Convert.ToInt32(fillRect.Height * -0.2));
                            using (var matRandom = new Mat(fillRect.Size, matTarget.Type()))
                            {
                                Cv2.Randu(matRandom, new Scalar(0, 0, 0), new Scalar(255, 255, 255));
                                matTarget[fillRect] = matRandom;
                            }
                        }
                    } while (true);
                }
            }
            catch (Exception e) {
                WriteError(new ErrorRecord(e, "Template Matching Failed", ErrorCategory.OperationStopped, null));
            }
        }
        #endregion Cmdlet Overrides
    }

    public class MatchedRect
    {
        public Rect Rect { get; set; }
        public float Similarity { get; set; }
    }
}