#FunctionField

## See also
* [DocumentField](DocumentField.htm)
* [ConstantField](ConstantField.htm)
* [PropertyChangedField](PropertyChangedField.htm)
* [AssemblyField](AssemblyField.htm)
* [Field](Field.htm)

## Overview
`FunctionField` is a primary and the easiest way to generate a dynamic values at runtime, this value be not be know at design time, as such using [ConstantField](ConstantField.htm) is imposible.

## Properties
* Name - The name of the field
* Scripts - The script used to evaluate the function, this must be a valid C# code. You can access the current context/entity using `item` keywords.

Example
returning today for `DateTime`
`DateTime.Today`

