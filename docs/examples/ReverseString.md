# ConcatSubString

## Пример кода на языке Swiston:

>_Данная программа складывает строку с подстрокой и считает длину полученной строки_

```
main {
    var s1: string = "Hello, World";
    var s2: string = "Swiston";

    var sub: string = substr(s1, 0, 5);   # "Hello"
    var result: string = sub + " " + s2;  # "Hello Swiston"

    var len: int = length(result);        # 13

    print(result);  # Вывод: Hello Swiston
    print(len);     # Вывод: 13
}
```