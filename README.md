# Xfa-Xml-Generator
I was tired of how to deal with XFA through C# and after some hours of work i made a XML generator based on XFA Fields structure. So you can fill the fields correctly and save them.

## Required

- This component use iTextSharp to work. Be sure to reference it;
- C# 6.0 or you can modify it to use with any previous version of C#. Use your imagination;

## Todo List

- Create a function to return XML as a string;
- Try to get the field label through XML nodes;
- Try to get checkbox or group items values through XML nodes;
- Improve code somehow;

## How to use

Call Xfa after PdfStamper has been called.

Example:
```
using Xfa_Xml_Generator;

...

using (MemoryStream memoryStream = new MemoryStream())
using (PdfReader pdfReader = new PdfReader(filename))
using (PdfStamper pdfStamper = new PdfStamper(pdfReader, memoryStream))
using (Xfa xfa = new Xfa(pdfStamper))
{
	//code here

	xfa.SaveChanges(); //It's very important. Call it before PdfStamper has been closed.

	pdfStamper.Close();
}
```

### Retrieving all the fields found

It will return a list of fields found in the document, the first one won't return button type fields because it isn't necessary but you could return it with the second one.

```
List<XfaModel> fields = xfa.GetFields();
List<XfaModel> allFields = xfa.GetAllFields();
```

It's important for you to know that some fields won't return even with these methods. Sometimes these fields don't have a name attribute so it will be kinda hard to treat but there's some methods here to try to treat these ones with Javascript.

### Xfa model representation

After retrieving the Xfa fields, it creates a model representation of all fields with it's properties, so you could work with it your way.

```
public string FieldName;
public string FieldType;
public string Value;
```

The Value property have it's own method to set it's value:

```
SetValue(string value)
```

### Javascript treatment

Here is some examples of how to use javascript to work with Xfa:

```
xfa.AddJavascript("var numericValue = 1;");

xfa.Javascript.SetVisibility(fields[0].FieldName, XfaVisibility.Presence);

xfa.Javascript.SetValue(fields[0].FieldName, "Testing Javascript RawValue");
```

See? It's not that complicated.

### The most important method

If somehow you're trying to work with this component and it seems not working properly, check if you're adding the next code before pdf is being closed:

```
xfa.SaveChanges();
```

It will generate a xml representation of all the fields retrieved, after you've working with them.
Javascript codes are added here too.

### Any doubts / issues found?

Please contact me or add a report ticket. Your help will improve this component for all the people who need it.
