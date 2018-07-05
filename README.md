[![Build Status](https://app.bitrise.io/app/5ea99524e3652396/status.svg?token=NcUJkieCZDnSWznXdbyHgQ&branch=master)](https://app.bitrise.io/app/5ea99524e3652396)

# Xfa-Xml-Generator

I was tired of dealing with XFA through C# and after some hours of work i made a XML generator based on XFA Fields structure.

So you can fill the fields correctly and save them.

## Required

- This component use iTextSharp. Be sure to reference it too;
- C# 6.0 or superior (OR you can modify it to use with any previous version of C#);

## Todo List

- Try to get the field label through XML nodes;
- Try to get checkbox or group items values through XML nodes;

## How to use

Call Xfa after PdfStamper has been called.

Example:

```csharp
using Xfa_Xml_Generator;

[...]

using (MemoryStream memoryStream = new MemoryStream())
using (PdfReader pdfReader = new PdfReader(filename))
using (PdfStamper pdfStamper = new PdfStamper(pdfReader, memoryStream))
using (Xfa xfa = new Xfa(pdfStamper))
{
    // CODE HERE

    xfa.SaveChanges(); // Call it before PdfStamper has been closed.

    pdfStamper.Close();
}
```

### Retrieving all the fields found

Returns a IEnumerable of fields found in the document, the first don't return button fields because it isn't necessary but you can return it with the second method.

```csharp
IEnumerable<XfaModel> fields = xfa.GetFields();
IEnumerable<XfaModel> allFields = xfa.GetAllFields();
```

It's important for you to know that some fields won't return even with these two methods. Sometimes these fields don't have a name attribute so it will be kinda hard to treat but there's some methods here to try to treat these ones with Javascript.

### Xfa model representation

After retrieving the Xfa fields, it creates a model representation of all fields.

Properties available:

```csharp
public string FieldName { get; private set; }
public string FieldType { get; private set; }
public string Value { get; private set; }
```

The Value property have it's own method to set it's value (if necessary):

```csharp
SetValue(string value)
```

### Javascript treatment

Here is a example of how to use javascript methods to work with Xfa:

```csharp
xfa.AddJavascript("var numericValue = 1;");

xfa.Javascript.SetVisibility(fields[0].FieldName, XfaVisibility.Presence);

xfa.Javascript.SetValue(fields[0].FieldName, "Testing Javascript RawValue");
```

### The most important method

If somehow you're trying to work with this component and it seems not working properly, check if you're adding the next code before pdf is being closed:

```csharp
xfa.SaveChanges();
```

It will generate a XML representation of all fields retrieved, after you had worked with them.

Javascript codes are added here too.

### Any doubts / issues found

Please add a report ticket. Your help will improve this component for everyone who needs it.
