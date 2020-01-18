using System;

using DataOnQ.Samples.Xamarin.Forms.Models;

namespace DataOnQ.Samples.Xamarin.Forms.ViewModels
{
	public class ItemDetailViewModel : BaseViewModel
	{
		public Item Item { get; set; }
		public ItemDetailViewModel(Item item = null)
		{
			Title = item?.Text;
			Item = item;
		}
	}
}
