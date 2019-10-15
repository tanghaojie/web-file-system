using System;
using System.Collections.Generic;

namespace WebFileSystem.Swagger {
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class SwaggerFormDataMethodAttribute : Attribute {
        public bool[] RequiredList { get; private set; }
        public string[] NameList { get; private set; }
        public string[] DescriptionList { get; private set; }
        public SwaggerFormDataMethodAttributeType[] TypeList { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public SwaggerFormDataMethodAttribute(bool[] requiredList, string[] nameList, string[] descriptionList, SwaggerFormDataMethodAttributeType[] typeList)
        {
            RequiredList = requiredList;
            NameList = nameList;
            DescriptionList = descriptionList;
            TypeList = typeList;
        }
    }
    public enum SwaggerFormDataMethodAttributeType {
        File,
        String,
    }
}
