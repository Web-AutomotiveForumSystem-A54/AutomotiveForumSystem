using AutomotiveForumSystem.Data;
using AutomotiveForumSystem.Models;
using AutomotiveForumSystem.Repositories.Contracts;

namespace AutomotiveForumSystem.Repositories
{
	public class TagsRepository : ITagsRepository
	{
		private readonly ApplicationContext applicationContext;

		public TagsRepository(ApplicationContext applicationContext)
		{
			this.applicationContext = applicationContext;
		}

		public Tag Create(Tag tag)
		{
			throw new NotImplementedException();
		}

		public bool Delete(Tag tag)
		{
			throw new NotImplementedException();
		}

		public IList<Tag> GetAll()
		{
			return applicationContext.Tags.ToList();
		}

		public Tag GetById(int id)
		{
			return applicationContext.Tags.FirstOrDefault(t => t.Id == id);
		}

		public Tag GetByName(string name)
		{
			return applicationContext.Tags.FirstOrDefault(t => t.Name == name);
		}

		public Tag Update(Tag tag)
		{
			throw new NotImplementedException();
		}
	}
}
