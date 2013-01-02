namespace Elmah.Masking
{
    using System;
    using System.Configuration;

    public class MaskedItemElement : ConfigurationElement
    {
        private static ConfigurationPropertyCollection _properties;
        private static readonly ConfigurationProperty _propNamespace;

        static MaskedItemElement()
		{
			MaskedItemElement._propNamespace = new ConfigurationProperty(MaskedItemElement.PropertyNames.Name, typeof(string), null, null, new StringValidator(1), ConfigurationPropertyOptions.IsRequired | ConfigurationPropertyOptions.IsKey);
			MaskedItemElement._properties = new ConfigurationPropertyCollection();
			MaskedItemElement._properties.Add(MaskedItemElement._propNamespace);
		}

        public MaskedItemElement(string name)
        {
            this.Name = name;
        }

        internal MaskedItemElement()
        {
        }

        [ConfigurationProperty(PropertyNames.Name, IsRequired = true, IsKey = true, DefaultValue = ""), StringValidator(MinLength = 1)]
        public string Name
        {
            get
            {
                return (string)base[PropertyNames.Name];
            }
            set
            {
                base[PropertyNames.Name] = value;
            }
        }

        protected override ConfigurationPropertyCollection Properties
        {
            get
            {
                return MaskedItemElement._properties;
            }
        }

        /// <summary>Compares the current instance to the passed <see cref="T:Elmah.DataProtection.MaskedItemElement" /> object.</summary>
		/// <returns>true if the two objects are identical.</returns>
        /// <param name="obscuredItem">A <see cref="T:Elmah.DataProtection.MaskedItemElement" /> object to compare to.</param>
        public override bool Equals(object obscuredItem)
		{
            MaskedItemElement obfuscatedItemElement = obscuredItem as MaskedItemElement;
            return obfuscatedItemElement != null && ((obfuscatedItemElement.Name == null && this.Name == null) || this.Name.Equals(obfuscatedItemElement.Name, StringComparison.OrdinalIgnoreCase));
		}

		/// <summary>Returns a hash value for the current instance.</summary>
		/// <returns>A hash value for the current instance.</returns>
		public override int GetHashCode()
		{
			return this.Name.GetHashCode();
		}

        public static class PropertyNames
        {
            public const string Name = "name";
        }
    }
}
