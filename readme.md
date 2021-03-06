# Classes and Structs
## Classes
### Reference types
A type defined as a `class` is a *reference type*. Variables of a reference type declared at runtime contain the value `null` until
explicitly initialised with an instance, or derivative, of the class.

When an object is created, memory is allocated on the managed heap for that object. The variable merely holds a memory address
pointing to the location of the object in memory.

### Declaring Classes
Declare a class with the `class` keyword followed by a unique identifier:
```
// [access modifier] - [class] - [identifier]
public class Animal
{
	// Fields, props, methods, events...
}
```

The class name is preceded by the access level modifier. The named identifier for the class follows the `class` keyword. The 
definition of the class is contained within the braces `{ ... }` and is where behaviour and data are defined. Any behaviour or
data defined on the class is a *class member*.

### Pass by reference
Classes are passed by reference, and so care should be taken not to use duplicate variables pointing at the same memory location.
```
Animal a1 = new Animal();
Animal a2 = a1; // 'a2' is pointing to the same Animal in memory as 'a1'
```

### Class Inheritance
Classes fully support inheritance. Any class not defined as `sealed` can be inherited from. Methods can be overridden by
derived classes if the base class methods are labelled `virtual`.

Inheritance occurs from *derivation*, a class declared by using a *base class* from which it inherits data and behaviour.
```
//			 this: BaseClass
public class Dog : Animal 
{
	// ...
}
```

Classes with a declared base class inherit all members of the base class except constructors. Only one base
class can be directly inherited from, this doesn't stop the base class from inheriting from another class. Classes
can implement one or more interfaces.

Classes can also be declared `abstract`, which contains `abstract` methods. An abstract class cannot be instantiated and only be
used through a derived class. Abstract methods have a method signature, but no method body.

### Class Example
See 'Animal.cs'.

## Struct
A `struct` object is a value type, a variable of a struct object holds a copy of the entire object because of this. An instance
of a struct does not need `new` operator to be instantiated.
```
public struct Animal
{
	public string Name;
	public string Breed;
	
	public Animal(string name, string breed)
	{
		Name = name;
		Breed = breed;
	}
}

public class Application
{
	static void Main()
	{
		// New Animal instance, memory allocated on thread stack
		Animal a1 = new Animal("Fred", "Canine");

		// New Animal instance, initialised with 'a1'
		Animal a2 = a1;
		a2.name = "Darcy";
	}
}
```

Struct instances are allocated memory on the thread stack, this memory is reclaimed with the type or method in which it is declared. Memory allocated
for class instances is reclaimed by the common language runtime when all references to an object have gone out of scope.

## Object Identity vs Value Equality
To determine if two class instances refer to the same memory location, use the static `Equals` method. To determine if
two instance fields in two struct instances have the same values, use the `ValueType.Equals` method as all structs
implicitly inherit from `System.ValueType`. This means methods can be called directly on the object.
```
Animal a1 = new Animal("Fred", "Canine");
Animal a2 = new Animal("Darcy", "Canine");

if (a1.Equals(a2)) ...
```

## Inheritance
Inheritance allows new classes to reuse, extend and modify behaviour defined in other classes. Classes whose members are inherited are called
*base classes* and the inheriting class is a *derived class*. Structs do not support inheritance but can implement interfaces.

### Abstract and virtual methods
Base classes with `virtual` methods declared can have those methods overridden in a derived class. Abstract members
must be overriden in any any non-abstract inheriting class. Abstract and virtual methods are the basis for *polymorphism*.

### Interfaces
Interfaces are reference types that define a set of members. All classes and structs implementing an interface
must implement its members. Default implementations may be provided for these members. Multiple interfaces can be implemented
by a class, but an interface can only derive from a single base class.

### Sealed classes
A class marked with `sealed` prevents inheritance, this also applies to class members.

### Derived Class hiding base class members
If a derived class declares a member of the same name and signature, use the `new` modifier to explicitly indicate that the member
isn't intended to override the base classes member.

## Polymorphism
Polymorphism is a Greek word meaning 'many-shaped/shapes', it has two distinct aspects in C#:
- Runtime objects of a derived class may be treated as objects of a base class in places such as method parameters and collections or arrays. The objects declared type is no longer identical to its runtime type.
- Base classes may define and implement virtual methods, and derived classes can override them. At runtime when client code calls a method, the CLR looks up the runtime type of the object, invoking the override of the virtual method. This means a method on a base class can be called and cause a derived class's version to be executed instead.

### Hiding with new
```
public class BaseClass
{
    public void DoWork() { WorkField++; }
    public int WorkField;
    public int WorkProperty
    {
        get { return 0; }
    }
}

public class DerivedClass : BaseClass
{
    public new void DoWork() { WorkField++; }
    public new int WorkField;
    public new int WorkProperty
    {
        get { return 0; }
    }
}
```

Access the hidden base class members by casting the instance of the derived class to an instance of the base classe:
```
DerivedClass B = new DerivedClass();
B.DoWork();  // Calls the new method.

BaseClass A = (BaseClass)B;
A.DoWork();  // Calls the old method.
```

### Calling a base class from a derived class
Use the `base` keyword:
```
public class Base
{
    public virtual void DoWork() {/*...*/ }
}
public class Derived : Base
{
    public override void DoWork()
    {
        //Perform Derived's work here
        //...
        // Call DoWork on base class
        base.DoWork();
    }
}
```

## Members
Classes and structs have members that represent their data and behaviour. Private members in base classes are inherited but not accessible
from derived classes.

A class or struct may contain:
- Fields
- Constants
- Properties
- Methods
- Events
- Operators
- Indexers
- Constructors
- Finalizers
- Nested Types

## Access Modifiers
Access modifiers control accessibility levels for code.
- `public` types or members are accessible by any code in the same assembly or another assembly that references it.
- `private` types or members are accessible only by code in the same `class` or `struct`.
- `protected` types or members are only accessible by code in the same `class` or in a derived `class`.
- `internal` types or members are accessible by any code in the same assembly, but not from another assembly.
- `protected internal` types or members are accessible by any code in the assembly it was declared in, or from within a derived `class` in another assembly.
- `private protected` types or members are accessible only within its declaring assembly, by code in the same `class` or in a derived `class`.

The default access modifier for classes and structs declared directly within a namespace is `internal`. These can either be `public` or `internal`.

Struct members, including nested classes and structs, are declarable as `public`, `internal`, or `private`. 
Class members, including nested classes and structs, can be `public`, `protected internal`, `protected`, `internal`, `private protected`, or `private`.
Class and struct members, including nested classes and structs, have `private` access by default.

Derived classes cannot have greater accessibility than their base types.

## Fields
*Fields* are variables of any type declared directly in a class or struct. A class or struct may have instance, static or a mix of both fields.

Fields should generally be used for variables with private or protected accessiblity. Data that a class exposes should be provided through 
methods, properties, and indexers. A private field that stores data exposed by a public property is called a *backing store*/*backing field*.

Fields can be declared readonly with the `readonly` keyword, this means the field can only be assigned a value during initialisation or in a constructor.

## Properties
A property is a member with a flexible mechanism for reading, writing or computing the value of a private field. Properties are accessible as
if they are public data members, but actually accessed with special methods called *accessors*. This allows easy data access and still helps to promote
the safety and flexibility of methods.

A `get` property accessor returns a property value while a `set` property accessor assigns a new value. The `value` keyword defines the value being assigned
by the `set` accessor. Proeprties can be read-write, read-only, or write-only.

The `set` accessor is commonly used for data validation before assigning a value to a private field.
```
class TimePeriod
{
   private double _seconds;

   public double Hours
   {
       get { return _seconds / 3600; }
       set {
          if (value < 0 || value > 24)
             throw new ArgumentOutOfRangeException(
                   $"{nameof(value)} must be between 0 and 24.");

          _seconds = value * 3600;
       }
   }
}
```

### Auto-implemented properties
If a property has a `get` and `set` accessor, they are auto-implemented.

### Using Properties
The `get` accessor is executed when a property is read whereas the `set` accessor is executed when a property is assigned a new value.

Properties are not classified as variables, thus a property cannot be passed as a `ref` or `out` parameter.

### Interface Properties
Interfaces can have properties declared on them, although they typically don't have a body. Accessors in interfaces are not auto-implemented.
```
public interface ISampleInterface
{
    // Property declaration:
    string Name
    {
        get;
        set;
    }
}
```

## Methods
```
abstract class Motorcycle
{
    // Anyone can call this.
    public void StartEngine() {/* Method statements here */ }

    // Only derived classes can call this.
    protected void AddGas(int gallons) { /* Method statements here */ }

    // Derived classes can override the base class implementation.
    public virtual int Drive(int miles, int speed) { /* Method statements here */ return 1; }

    // Derived classes must implement this.
    public abstract double GetTopSpeed();
}
```

### Passing by reference vs passing by value
By default, when an instance of a value type is passed to a method a copy is passed instead of the original instance. To pass a value-type instance
by reference, use the `ref` keyword.

When a reference type is passed to a method, a reference to the object is passed. Changes to the object passed to the method will take affect
outside the method. Reference types are created with the `class` keyword most commonly.

### Return values
A `return` statement will return a value, or as of C# 7.0, can return by reference if the `ref` keyword is in the method signature:
```
public ref double GetEstimatedDistance()
{
    return ref estDistance;
}
```

To use a value returned by reference from a method, declare a `ref` local variable if intending to modify its value:
```
ref int distance = GetEstimatedDistance();
```

### Local Functions
Local functions are private methods of a type nested in another member, they are only callable from their containing member. Local functions
can be declared in and called from:
- Methods
- Constructors
- Property accessors
- Event accessors
- Anonymous methods
- Lambda expressions
- Finalizers
- Other local functions

Local functions cannot be declared inside an expression-bodied member. Local functions are always `private`.

#### Syntax
```
<modifiers> <return-type> <method-name> <parameter-list>
```

The following modifiers can be used:
- async
- unsafe
- static (C# 8.0 and later), cannot capture local vars or instance state
- extern (C# 9.0 and later), must be static

Example:
```
private static string GetText(string path, string filename)
{
     var reader = File.OpenText($"{AppendPathSeparator(path)}{filename}");
     var text = reader.ReadToEnd();
     return text;

     string AppendPathSeparator(string filepath)
     {
        return filepath.EndsWith(@"\") ? filepath : filepath + @"\";
     }
}
```

Attributes can be applied to a local function, its parameters and type parameters as of C# 9.0:
```
#nullable enable
private static void Process(string?[] lines, string mark)
{
    foreach (var line in lines)
    {
        if (IsValid(line))
        {
            // Processing logic...
        }
    }

    bool IsValid([NotNullWhen(true)] string? line)
    {
        return !string.IsNullOrEmpty(line) && line.Length >= mark.Length;
    }
}
```

### Implementing and calling a custom extendsion method
1. Define a static class to contain the extension method
2. Implement the extension method as a static method with at least the same visibility as the containing class
3. The first param of the method specifies the type that the method operates on; it must be preceded with the `this` modifier
4. Add a `using` directive to specify the namespace that contains the extension method class
5. Call methods as if they were instance methods on the type
```
using System.Linq;
using System.Text;
using System;

namespace CustomExtensions
{
    // Extension methods must be defined in a static class.
    public static class StringExtension
    {
        // This is the extension method.
        // The first parameter takes the "this" modifier
        // and specifies the type for which the method is defined.
        public static int WordCount(this String str)
        {
            return str.Split(new char[] {' ', '.','?'}, StringSplitOptions.RemoveEmptyEntries).Length;
        }
    }
}
namespace Extension_Methods_Simple
{
    // Import the extension method namespace.
    using CustomExtensions;
    class Program
    {
        static void Main(string[] args)
        {
            string s = "The quick brown fox jumped over the lazy dog.";
            // Call the method as if it were an
            // instance method on the type. Note that the first
            // parameter is not specified by the calling code.
            int i = s.WordCount();
            System.Console.WriteLine("Word count of s is {0}", i);
        }
    }
}
```

## Finalizers
A finalizer is also known as a **destructor**, they are commonly used to perform necessary final clean-up when a class instance is being collected
by the garbage collector.

- Can only be used in classes
- A class can have only one finalizer
- Cannot be inherited or overloaded
- Cannot be called (invoked automatically)
- Does not take modifiers or have parameters
```
class First
{
    ~First()
    {
        System.Diagnostics.Trace.WriteLine("First's destructor is called.");
    }
}

class Second : First
{
    ~Second()
    {
        System.Diagnostics.Trace.WriteLine("Second's destructor is called.");
    }
}

class Third : Second
{
    ~Third()
    {
        System.Diagnostics.Trace.WriteLine("Third's destructor is called.");
    }
}

class TestDestructors
{
    static void Main()
    {
        Third t = new Third();
    }
}
/* Output (to VS Output Window):
    Third's destructor is called.
    Second's destructor is called.
    First's destructor is called.
*/
```