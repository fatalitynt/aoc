def main():
    filename = "_input.txt"
    with open(filename) as file:
        lines = [line.rstrip() for line in file]
    solve(lines, 2)
    solve(lines, 12)


def solve(lines: list, size: int):
    res = 0
    for line in lines:
        res += findMaxWithSize([int(c) for c in line], size)
    print("res", res)


def findMaxWithSize(a: list, size: int) -> int:
    start = 0
    res = 0
    while size > 0:
        size -= 1
        maxIdx = findMaxIdxInRange(a, start, len(a) - size)
        res = res * 10 + a[maxIdx]
        start = maxIdx + 1
    return res


def findMaxIdxInRange(a: list, l: int, r: int) -> int:
    j = l
    for i in range(l, r):
        if a[i] > a[j]:
            j = i
    return j


if __name__ == "__main__":
    main()

"""
What I learned?
- Simple list comprehension to make array from string [int(c) for c in line]
"""
