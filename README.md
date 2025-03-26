# Unity.DS

A collection of zero allocation data structure for Unity, I needed a `LinkedList` dropped-in replacement and surprisingly couldn't find any that was truly alloc free. I might add more.

## Installation

Add the dependency to your `manifest.json`

```json
{
  "dependencies": {
    "jd.boiv.in.ds": "https://github.com/starburst997/Unity.DS.git"
  }
}
```

## Usage

```csharp
using JD.DS;
var list = new LinkedList<Enemy>(100);
var node = list.AddLast(enemy);
foreach (var element in list) Debug.Log($"Enemy: {element.Name}");
```

## Data Structure

- `LinkedList`: Drop-in replacement to the original `LinkedList`

## TODO

- Add more examples
- More data structure
- Better readme

## Credit

- `LinkedList` originally from [EllanJiang/GameFramework](https://github.com/EllanJiang/GameFramework)