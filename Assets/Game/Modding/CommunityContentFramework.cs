using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Modding
{
    public sealed class CommunityContentDefinition
    {
        public string ContentId { get; }

        public string ContentName { get; }

        public string AuthorId { get; }

        public bool Approved { get; private set; }

        public bool Published { get; private set; }

        public CommunityContentDefinition(
            string contentId,
            string contentName,
            string authorId)
        {
            ContentId =
                contentId ?? string.Empty;

            ContentName =
                contentName ?? string.Empty;

            AuthorId =
                authorId ?? string.Empty;

            Approved = false;
            Published = false;
        }

        public bool Approve()
        {
            if (Approved)
            {
                return false;
            }

            Approved = true;

            return true;
        }

        public bool Publish()
        {
            if (!Approved ||
                Published)
            {
                return false;
            }

            Published = true;

            return true;
        }

        public bool Unpublish()
        {
            if (!Published)
            {
                return false;
            }

            Published = false;

            return true;
        }
    }

    public sealed class CommunityContentFramework
    {
        private readonly Dictionary<
            string,
            CommunityContentDefinition> content =
            new Dictionary<
                string,
                CommunityContentDefinition>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int ContentCount =>
            content.Count;

        public int PublishedContentCount
        {
            get
            {
                int count = 0;

                foreach (CommunityContentDefinition item
                         in content.Values)
                {
                    if (item.Published)
                    {
                        count++;
                    }
                }

                return count;
            }
        }

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            content.Clear();
            Initialized = true;

            return true;
        }

        public bool RegisterContent(
            string contentId,
            string contentName,
            string authorId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(contentId) ||
                string.IsNullOrWhiteSpace(contentName) ||
                string.IsNullOrWhiteSpace(authorId))
            {
                return false;
            }

            string id =
                contentId.Trim();

            if (content.ContainsKey(id))
            {
                return false;
            }

            content.Add(
                id,
                new CommunityContentDefinition(
                    id,
                    contentName.Trim(),
                    authorId.Trim()));

            return true;
        }

        public bool ApproveContent(
            string contentId)
        {
            CommunityContentDefinition item =
                GetContent(contentId);

            return item != null &&
                   item.Approve();
        }

        public bool PublishContent(
            string contentId)
        {
            CommunityContentDefinition item =
                GetContent(contentId);

            return item != null &&
                   item.Publish();
        }

        public bool UnpublishContent(
            string contentId)
        {
            CommunityContentDefinition item =
                GetContent(contentId);

            return item != null &&
                   item.Unpublish();
        }

        public CommunityContentDefinition GetContent(
            string contentId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(contentId))
            {
                return null;
            }

            content.TryGetValue(
                contentId.Trim(),
                out CommunityContentDefinition item);

            return item;
        }

        public IReadOnlyCollection<
            CommunityContentDefinition>
            GetContent()
        {
            return content.Values;
        }

        public void Reset()
        {
            content.Clear();
            Initialized = false;
        }
    }
}
