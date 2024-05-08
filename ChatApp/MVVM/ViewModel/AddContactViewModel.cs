using ChatApp.Core;
using ChatApp.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.MVVM.ViewModel
{
    internal class AddContactViewModel : ObservableObject
    {
		public string contactId;

		public string ContactId
		{
			get { return contactId; }
			set { contactId = value
					; OnPropertyChanged();
			}
		}

		public RelayCommand AddContactCommand {  get; set; }


		public AddContactViewModel()
		{
			AddContactCommand = new RelayCommand(o => ServerUtils.AddContact(contactId));
		}

	}
}
