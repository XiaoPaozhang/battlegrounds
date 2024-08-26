using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

public class ObservableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, INotifyCollectionChanged, ICollection<KeyValuePair<TKey, TValue>>
{
  public event NotifyCollectionChangedEventHandler CollectionChanged;

  protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
  {
    CollectionChanged?.Invoke(this, e);
  }

  public new void Add(TKey key, TValue value)
  {
    base.Add(key, value);
    OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, new KeyValuePair<TKey, TValue>(key, value), Count));
  }

  public new bool Remove(TKey key)
  {
    TValue value;
    if (TryGetValue(key, out value))
    {
      var result = base.Remove(key);
      OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, new KeyValuePair<TKey, TValue>(key, value), Count));
      return result;
    }
    return false;
  }

  public new void Clear()
  {
    base.Clear();
    OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
  }

  public void Add(KeyValuePair<TKey, TValue> item)
  {
    Add(item.Key, item.Value);
  }

  public bool Remove(KeyValuePair<TKey, TValue> item)
  {
    return Remove(item.Key);
  }

  public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
  {
    foreach (var pair in this)
    {
      array[arrayIndex++] = pair;
    }
  }

  public new IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
  {
    return base.GetEnumerator();
  }

  public new int Count => base.Count;

  public bool IsReadOnly => ((ICollection<KeyValuePair<TKey, TValue>>)this).IsReadOnly;

  IEnumerator IEnumerable.GetEnumerator()
  {
    return GetEnumerator();
  }
}