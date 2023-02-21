// Copyright (c) Microsoft Corporation. All Rights Reserved.
// Licensed under the MIT License.

namespace Todo.RestApi.Models;

/// <summary>
/// A type representing a page of items.
/// </summary>
/// <typeparam name="T">The type of the items.</typeparam>
public class Page<T>
{
    public Page(IEnumerable<T>? items, bool hasMoreItems, Uri? nextLink)
    {
        Items = items;
        HasMoreItems = hasMoreItems;
        NextLink = HasMoreItems ? nextLink : null;
    }

    /// <summary>
    /// The list of items in this result.
    /// </summary>
    public IEnumerable<T>? Items { get; set; }

    /// <summary>
    /// If <c>true</c>, more items can be retrieved.
    /// </summary>
    public bool HasMoreItems { get; set; }

    /// <summary>
    /// If there are more items, then the link to the next page of items.
    /// </summary>
    public Uri? NextLink { get; set; }
}
