# Umbrella.Infrastructure.FileStorage

## Usage

for the usage of the library, please refer to extension methods.

<b>Standard usage</b>

```c#
services.UseFileStorageFromFileSystem(@"C:\Temp\MyFolder");
```

or if you need to store data on Google Cloud Storage:

```c#
services.UseFileStorageFromGoogle("my-bucket-name");
```

than, once you inject this depedency to your component, use such snippets:

```c#
readonly IFileStorage _Storage;

public MyComponent(IFileStorage storage)
{
    this._Storage = storage ?? throw new ArgumentNullException(nameof(storage));
    if(!this._Storage.IsScanPerformed)
        this._Storage.ScanStorageTree();
}

```

it's important ot scan the tree to get the content information ready for accessing it.
