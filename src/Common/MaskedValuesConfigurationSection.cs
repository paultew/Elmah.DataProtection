namespace Elmah.Masking
{
    using System.ComponentModel;
    using System.Configuration;

    public class MaskedValuesConfigurationSection : ConfigurationSection
    {
        public const string SectionName = "obscured";
        public const string AspxAuthCookie = ".ASPXAUTH";

        private const bool RemoveAspxAuthDefault = true;
        private const string ReplacementTextDefault = "(REMOVED)";

        public MaskedValuesConfigurationSection()
        {
            this.RemoveAspxAuth = RemoveAspxAuthDefault;
            this.ReplacementText = ReplacementTextDefault;
            this.Cookies = new MaskedItemCollection();
            this.FormVariables = new MaskedItemCollection();
            this.ServerVariables = new MaskedItemCollection();
        }

        [ConfigurationProperty(PropertyNames.RemoveAspxAuth, DefaultValue = RemoveAspxAuthDefault, IsRequired = false, IsKey = false)]
        public bool RemoveAspxAuth
        {
            get { return (bool)base[PropertyNames.RemoveAspxAuth]; }
            set { base[PropertyNames.RemoveAspxAuth] = value; }
        }

        [ConfigurationProperty(PropertyNames.ReplacementText, DefaultValue = ReplacementTextDefault, IsRequired = false, IsKey = false)]
        public string ReplacementText
        {
            get { return (string)base[PropertyNames.ReplacementText]; }
            set { base[PropertyNames.ReplacementText] = value; }
        }

        [Description("The cookies to replace.")]
        [ConfigurationProperty(ElementNames.Cookies, IsRequired = false, IsKey = false)]
        public MaskedItemCollection Cookies
        {
            get
            {
                return (MaskedItemCollection)base[ElementNames.Cookies];
            }
            set
            {
                base[ElementNames.Cookies] = value;
            }
        }

        [Description("The form variables to replace.")]
        [ConfigurationProperty(ElementNames.FormVariables, IsRequired = false, IsKey = false)]
        public MaskedItemCollection FormVariables
        {
            get
            {
                return (MaskedItemCollection)base[ElementNames.FormVariables];
            }
            set
            {
                base[ElementNames.FormVariables] = value;
            }
        }

        [Description("The server variables to replace.")]
        [ConfigurationProperty(ElementNames.ServerVariables, IsRequired = false, IsKey = false)]
        public MaskedItemCollection ServerVariables
        {
            get
            {
                return (MaskedItemCollection)base[ElementNames.ServerVariables];
            }
            set
            {
                base[ElementNames.ServerVariables] = value;
            }
        }

        public static class ElementNames
        {
            public const string Cookies = "cookies";
            public const string FormVariables = "formVariables";
            public const string ServerVariables = "serverVariables";
        }

        public static class PropertyNames
        {
            public const string RemoveAspxAuth = "removeAspxAuth";
            public const string ReplacementText = "replacementText";
        }
    }
}
