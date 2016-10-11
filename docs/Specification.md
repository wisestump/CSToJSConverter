# The way it works

Converter accepts the root of C# AST (`CSharpSyntaxNode`) as an input and traverses it in depth-first order, 
forming JavaScript source code. Every node in the tree must be convertable. If during tree processing converter 
meets node, that it doesn't know how to convert, it throws exception.

# What it can convert

## Statements

### Method declarations
Method declarations are converted to JS functions. Access and member modifiers, parameter modifiers are ignored.
Optional parameters don't have their initial values.

### Conditional operator
If statement is transferred without any significant changes because it has the same syntax.

### Cycles
- `for` cycle
- `while` cycle
- `do-While` cycle

### Local variable definition
- `var` statements have the same syntax, so no changes presented. If `var` declares multiple variables, they are divided into
several `var` statements. 
- `Type variable` declarations are converted into `var variable` statements.

Initial values are saved during conversion.

## Expressions

### Basic expressions
Converter is capable of processing identifiers, literals, parenthesized expressions. Qualified identifiers are not suppurted.

### Binary expressions
Converter supports follow C# binary operators: 
- Arithmetic: `+` `-` `*` `/` `%`
- Bitwise: `>>` `<<` `|` `&`
- Logical: `&&` `||` `^` `<` `<=` `>` `>=` `!=` `==`. No logical `|` and `&`.

### Unary expressions
Prefix and postfix unary expressions are converted without any changes.
