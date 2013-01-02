namespace Elmah.Masking
{
    using System.Configuration;

    [ConfigurationCollection(typeof(MaskedItemElement))]
    public class MaskedItemCollection : ConfigurationElementCollection
    {
        /// <summary>Gets or sets the <see cref="T:Elmah.DataProtection.MaskedItemElement" /> object at the specified index in the collection.</summary>
        /// <returns><see cref="T:System.Web.Configuration.NamespaceInfo" /> object at the specified index, or null if there is no object at that index.</returns>
        /// <param name="index">The index of a <see cref="T:Elmah.DataProtection.MaskedItemElement" /> object in the collection.</param>
        public MaskedItemElement this[int index]
        {
            get
            {
                return (MaskedItemElement)base.BaseGet(index);
            }
            set
            {
                if (base.BaseGet(index) != null)
                {
                    base.BaseRemoveAt(index);
                }
                this.BaseAdd(index, value);
            }
        }

        /// <summary>Removes the <see cref="T:Elmah.DataProtection.MaskedItemElement" /> object with the specified key from the collection.</summary>
        /// <param name="s">The namespace of a <see cref="T:Elmah.DataProtection.MaskedItemElement" /> object to remove from the collection.</param>
        /// <exception cref="T:System.Configuration.ConfigurationException">
        /// There is no <see cref="T:Elmah.DataProtection.MaskedItemElement" /> object with the specified key in the collection.
        ///
        /// - or -
        ///
        /// The element has already been removed.
        ///
        /// - or -
        ///
        /// The collection is read-only.
        /// </exception>
        public void Remove(string s)
        {
            base.BaseRemove(s);
        }

        /// <summary>Removes a <see cref="T:Elmah.DataProtection.MaskedItemElement" /> object from the specified index in the collection.</summary>
        /// <param name="index">The index of a <see cref="T:Elmah.DataProtection.MaskedItemElement" /> object to remove from the collection.</param>
        /// <exception cref="T:System.Configuration.ConfigurationException">There is no <see cref="T:Elmah.DataProtection.MaskedItemElement" /> object at the specified index in the collection.
        ///
        /// - or -
        ///
        /// The element has already been removed.
        ///
        /// - or -
        ///
        /// The collection is read-only.
        /// </exception>
        public void RemoveAt(int index)
        {
            base.BaseRemoveAt(index);
        }

        /// <summary>Adds a <see cref="T:Elmah.DataProtection.MaskedItemElement" /> object to the collection.</summary>
        /// <param name="element">A <see cref="T:Elmah.DataProtection.MaskedItemElement" /> object to add to the collection.</param>
        /// <exception cref="T:System.Configuration.ConfigurationException">The <see cref="T:Elmah.DataProtection.MaskedItemElement" /> object to add already exists in the collection or the collection is read-only.</exception>
        public void Add(MaskedItemElement element)
        {
            this.BaseAdd(element);
        }

        #region Overrides of ConfigurationElementCollection

        /// <summary>
        /// Creates a new <see cref="T:Elmah.DataProtection.MaskedItemElement"/>.
        /// </summary>
        /// <returns>
        /// A new <see cref="T:System.Configuration.ConfigurationElement"/>.
        /// </returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new MaskedItemElement();
        }

        /// <summary>
        /// Gets the element key for a specified configuration element when overridden in a derived class.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Object"/> that acts as the key for the specified <see cref="T:Elmah.DataProtection.MaskedItemElement"/>.
        /// </returns>
        /// <param name="element">The <see cref="T:Elmah.DataProtection.MaskedItemElement"/> to return the key for. </param>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((MaskedItemElement)element).Name;
        }

        #endregion
    }
}
