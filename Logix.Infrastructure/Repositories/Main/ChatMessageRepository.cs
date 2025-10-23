using Logix.Application.Interfaces.IRepositories.Main;
using Logix.Domain.Main;
using Logix.Infrastructure.DbContexts;

namespace Logix.Infrastructure.Repositories.Main
{
    public class ChatMessageRepository : GenericRepository<ChatMessage>, IChatMessageRepository
    {
        private readonly ApplicationDbContext context;

        public ChatMessageRepository(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }


    }
}
