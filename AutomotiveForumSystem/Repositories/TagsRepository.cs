using AutomotiveForumSystem.Data;
using AutomotiveForumSystem.Exceptions;
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
			applicationContext.Tags.Add(tag);
			return tag;
		}

		public bool Delete(Tag tag)
		{
			applicationContext.Tags.Remove(tag);
			return true;
		}

		public IList<Tag> GetAll()
		{
			return applicationContext.Tags.ToList();
		}

		public Tag GetById(int id)
		{
			return applicationContext.Tags.FirstOrDefault(t => t.Id == id)
				?? throw new EntityNotFoundException($"Tag with id {id} not found");
		}

		public Tag GetByName(string name)
		{
			return applicationContext.Tags.FirstOrDefault(t => t.Name == name)
				?? throw new EntityNotFoundException($"Tag with name {name} not found");
		}

		public Tag Update(Tag tag)
		{
			applicationContext.Update(tag);
			return tag;
		}
	}
}
