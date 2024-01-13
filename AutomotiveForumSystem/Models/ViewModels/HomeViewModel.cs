namespace AutomotiveForumSystem.Models.ViewModels
{
	public class HomeViewModel
	{
		public List<CategoryLabelViewModel> Categories { get; set; } = new List<CategoryLabelViewModel>();
		public List<PostPreViewModel> Posts { get; set; } = new List<PostPreViewModel>();
	}
}
