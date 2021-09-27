# DictionaryFlushPartialCache
Sitecore Dictionary Flush Partial Cache

If you use Partial Html Cache and Sitecore Dictionary items the Cache will not flush on changes.
For flushing Html cache on Dictionary change, There are multiple ways to use the Dictionary but this is about when using Sitecore Items in master/web database and editors can change the value. 
This workaround contains:
a processor in the item:save event that do a clear on the cache when a dictionary item is saved. 
Flush the entire cache or specify which renderings you want to clear, auto detect which rendering are using the dictionary is complex to write. 
So specify or just flush the whole cache. This goes against the idea of the partial HTML cache, but it is useful to be able to completely empty the cache if you need to. 
Usually the dictionary is not modified much and extra permissions are needed for this.

