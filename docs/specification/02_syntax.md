# Синтаксис языка Swiston

## 1. Общая структура программы

| Свойство                 | Требование                  |
| ------------------------ | --------------------------- |
| Файл                     | Один                        |
| Точка входа              | Ровно один блок `main`      |
| Пользовательские функции | Не поддерживаются в Эпике 1 |


------------------------------------------------------------------------

## 2. Приоритет и ассоциативность операторов

От высшего к низшему:

| Приоритет | Операторы     | Ассоциативность |
| --------- | ------------- | --------------- |
| 1         | `*`, `/`, `%` | Левая           |
| 2         | `+`, `-`      | Левая           |

------------------------------------------------------------------------

## 3. Полная ISO EBNF-грамматика

``` ebnf
program =
    "main",
    block ;

block =
    "{",
    { statement },
    "}" ;

statement =
      var_decl
    | const_decl
    | assignment
    | print_stmt
    | read_stmt ;

var_decl =
    "var",
    identifier,
    ":",
    type,
    [ "=", expression ],
    ";" ;

const_decl =
    "const",
    identifier,
    ":",
    type,
    "=",
    expression,
    ";" ;

assignment =
    identifier,
    "=",
    expression,
    ";" ;

print_stmt =
    "print",
    "(",
    expression,
    ")",
    ";" ;

read_stmt =
    "read",
    "(",
    identifier,
    ")",
    ";" ;

type =
      "int"
    | "float"
    | "string" ;

expression = equality ;

equality =
    additive,
    { ( "==" | "!=" ), additive } ;

additive =
    multiplicative,
    { ( "+" | "-" ), multiplicative } ;

multiplicative =
    primary,
    { ( "*" | "/" | "%" ), primary } ;

primary =
      literal
    | identifier
    | "(",
      expression,
      ")" ;

literal =
      int_literal
    | float_literal
    | string_literal ;
```