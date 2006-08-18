#region Disclaimer/Info
///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at SourceForge at http://sourceforge.net/projects/subtext
// The development mailing list is at subtext-devs@lists.sourceforge.net 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////
#endregion

using System;
using System.Collections.Generic;
using Subtext.Framework.Data;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Syndication;

namespace Subtext.Framework.Syndication
{
	/// <summary>
	/// RssCommentHandler is a proposed extention to the CommentApi. This is still beta/etc.
	/// The Main Rss feed now contains an element for each entry, which will generate a rss feed 
	/// containing the comments for each post.
	/// </summary>
	public class RssCommentHandler : EntryCollectionHandler
	{
		protected Entry ParentEntry;
        protected IList<Entry> Comments;
        IList<Entry> comments;

		/// <summary>
		/// Gets the feed entries.
		/// </summary>
		/// <returns></returns>
        protected override IList<Entry> GetFeedEntries()
		{
			if(ParentEntry == null)
			{
				ParentEntry = Cacher.GetEntryFromRequest(CacheDuration.Short);
			}

			if(ParentEntry != null && Comments == null)
			{
				Comments = Cacher.GetComments(ParentEntry, CacheDuration.Short);
			}

			return Comments;
		}


		/// <summary>
		/// Builds the feed using delta encoding if it's true.
		/// </summary>
		/// <returns></returns>
		protected override CachedFeed BuildFeed()
		{
			CachedFeed feed;

			comments = GetFeedEntries();
			if(comments == null)
				comments = new List<Entry>();

		
			feed = new CachedFeed();
			CommentRssWriter crw = new CommentRssWriter(comments,ParentEntry);
			if(comments.Count > 0)
			{
				feed.LastModified = this.ConvertLastUpdatedDate(comments[comments.Count-1].DateCreated);
			}
			else
			{
				feed.LastModified = this.ParentEntry.DateCreated;
			}
			feed.Xml = crw.Xml;
			return feed;
		}

		protected override bool IsLocalCacheOK()
		{
			string dt = LastModifiedHeader;
			if(dt != null)
			{
				comments = GetFeedEntries();

				if(comments != null && comments.Count > 0)
				{
					return DateTime.Compare(DateTime.Parse(dt), this.ConvertLastUpdatedDate(comments[comments.Count-1].DateCreated)) == 0;
				}
			}
			return false;			
		}

		protected override BaseSyndicationWriter SyndicationWriter
		{
			get
			{
				return new CommentRssWriter(comments, ParentEntry);
			}
		}
	}
}
