using AutomotiveForumSystem.Models;

namespace AutomotiveForumSystem.Services.Contracts
{
	public interface ITagsService
	{
		IList<Tag> GetAll();
		Tag GetById(int id);
		Tag GetByName(string name);
		IList<Tag> TryAddTags(List<Tag> tags);
		Tag Create(Tag tag);
		Tag Update(Tag tag);
		bool Delete(Tag tag);
	}
}
