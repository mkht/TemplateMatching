using OpenCvSharp;
using System;
using System.Collections.Generic;
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
        /// You can specifies threshold between 0.0 to 1.0 (greater is best match)
        /// The default threshold is 0.95
        /// </summary>
        [Parameter()]
        [ValidateRange(0.0, 1.0)]
        public double Threshold { get; set; } = 0.95;
        #endregion Parameters

        #region Private Data
        private List<MatchedRect> MatchedList;
        private int templateHeight;
        private int templateWidth;
        #endregion Private Data

        #region Cmdlet Overrides
        protected override unsafe void ProcessRecord()
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
                    // Invoke template matching
                    Cv2.MatchTemplate(matTarget, matTemplate, result, TemplateMatchModes.CCoeffNormed);

                    templateHeight = matTemplate.Height;
                    templateWidth = matTemplate.Width;
                    MatchedList = new List<MatchedRect>();

                    //Extract all matched points that has more similar than the threshold
                    result.ForEachAsFloat(Extract);
                }
            }
            catch (Exception e) {
                WriteError(new ErrorRecord(e, "Template Matching Failed", ErrorCategory.OperationStopped, null));
            }

            // Return object
            WriteObject(MatchedList);
        }
        #endregion Cmdlet Overrides

        private unsafe void Extract(float* value, int* position)
        {
            if (*value >= Threshold)
            {
                var rect = new MatchedRect();
                rect.Rect = new Rect(position[0], position[1], templateWidth, templateHeight);
                rect.Similarity = *value;
                MatchedList.Add(rect);
            }
        }

    }

    public class MatchedRect
    {
        public Rect Rect { get; set; }
        public float Similarity { get; set; }
    }
}
