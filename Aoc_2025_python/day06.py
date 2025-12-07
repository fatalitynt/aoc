import math


def main():
    filename = "_input.txt"
    with open(filename) as file:
        lines = [line for line in file]
    print("p1", solve1(lines))
    print("p2", solve2(lines))


def solve1(lines: list) -> int:
    lines = [x.split() for x in lines]
    res = [int(x) for x in lines[0]]
    for line in lines[1:-1]:
        for i in range(len(line)):
            if lines[-1][i] == "*":
                res[i] *= int(line[i])
            else:
                res[i] += int(line[i])
    return sum(res)


def solve2(lines: list) -> int:
    res = 0
    w = len(lines[0])
    values = []
    for x in range(w - 1, -1, -1):
        val, c = scan_col(lines, x)
        if val != 0:
            values.append(val)
        if c == "*":
            res += math.prod(values)
            values = []
        if c == "+":
            res += sum(values)
            values = []
    return res


def scan_col(lines: list, x: int) -> tuple[int, str]:
    val = 0
    c = " "
    for y in range(0, len(lines)):
        c = lines[y][x]
        if c.isdigit():
            val = val * 10 + int(c)
    return val, c


if __name__ == "__main__":
    main()

"""
What I learned?
 - split() without args removes empty entries automatically
 - To create an array of given length: [None] * n
 - Reverse loop: use range(len(arr)-1, -1, -1) or reversed(arr)
 - Check if char is digit: c.isdigit() or '0' <= c <= '9'
 - Type hint for tuple return: use tuple[int, str], not (int, str)
 - Tuple returned by a function can be deconstructed: a, b = func()
 - To multiply array elements: use math.prod(a)
"""
