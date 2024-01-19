using AutomotiveForumSystem.Models;
using AutomotiveForumSystem.Repositories.Contracts;
using AutomotiveForumSystem.Services.Contracts;

namespace AutomotiveForumSystem.Services
{
	public class TagsService : ITagsService
	{
		private readonly ITagsRepository tagsRepository;

		public TagsService(ITagsRepository tagsRepository)
		{
			this.tagsRepository = tagsRepository;
		}

		public Tag Create(Tag tag)
		{
			return tagsRepository.Create(tag);
		}

		public bool Delete(Tag tag)
		{
			return tagsRepository.Delete(tag);
		}

		public IList<Tag> GetAll()
		{
			return tagsRepository.GetAll();
		}

		public Tag GetById(int id)
		{
			return tagsRepository.GetById(id);
		}

		public Tag GetByName(string name)
		{
			return tagsRepository.GetByName(name);
		}

		public Tag Update(Tag tag)
		{
			throw new NotImplementedException();
		}
	}
}
