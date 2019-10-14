using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace SafeSportChat.Behaviors
{
    public class MaskPhoneBehavior : Behavior<Entry>
    {
        //We take the mask format that is specified form the object in XAML
        private string _mask = "";
        public string Mask
        {
            get => _mask;
            set
            {
                _mask = value;
            }
        }

        private void OnEntryTextChanged(object sender, TextChangedEventArgs args)
        {
            var entry = sender as Entry;
            //To allow letters, remove property Keyboard="Telephone" from the object in XAMl 
            string text = entry.Text;

            if (!string.IsNullOrWhiteSpace(Mask))
            {
                // Adding MaxLength (constrain the imput text) 
                if (text.Length == _mask.Length)
                {
                    entry.MaxLength = _mask.Length;
                }
            }

            // We are going to use this code onlu when the user inser characters
            // When the user erase character this code is not executed
            bool IsInputChanged = (args.OldTextValue == null) || (args.OldTextValue.Length <= args.NewTextValue.Length);
            if (IsInputChanged)
            {

                for (int i = Mask.Length; i >= text.Length; i--)
                {
                    // Adding charcater into the mask format
                    int positionToInsertChar = text.Length - 1;
                    if (Mask[positionToInsertChar] != 'X')
                    {
                        text = text.Insert(positionToInsertChar, Mask[positionToInsertChar].ToString());
                    }
                }
            }
            entry.Text = text;
        }

        protected override void OnAttachedTo(Entry entry)
        {
            entry.TextChanged += OnEntryTextChanged;
            base.OnAttachedTo(entry);
        }
        protected override void OnDetachingFrom(Entry entry)
        {
            entry.TextChanged -= OnEntryTextChanged;
            base.OnDetachingFrom(entry);
        }
    }
}

