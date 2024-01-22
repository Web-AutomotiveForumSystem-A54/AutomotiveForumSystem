using AutomotiveForumSystem.Models;

namespace AutomotiveForumSystem.Repositories.Contracts
{
	public interface ITagsRepository
	{
		IList<Tag> GetAll();
		Tag GetById(int id);
		Tag GetByName(string name);
		Tag Create(Tag tag);
		Tag Update(Tag tag);
		bool Delete(Tag tag);
		bool IsTagPresent(Tag tag);
	}
}
