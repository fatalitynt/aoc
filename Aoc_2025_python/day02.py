def main():
    filename = "_input.txt"
    with open(filename) as file:
        lines = [line.rstrip() for line in file]
    solve(lines, isInvalid)
    solve(lines, isInvalid2)


def solve(lines: list, invalid):
    res = 0
    parts = lines[0].split(",")
    for p in parts:
        lr = p.split("-")
        left = int(lr[0])
        right = int(lr[1])
        for i in range(left, right + 1):
            if invalid(i):
                res += i
    print("res", res)


def isInvalid(x: int) -> bool:
    s = str(x)
    if len(s) % 2 != 0:
        return False
    sl = s[: len(s) // 2]
    sr = s[len(s) // 2 :]
    return sl == sr


def isInvalid2(x: int) -> bool:
    starts = []
    s = str(x)
    for i in range(1, len(s)):
        if s[i] == s[0]:
            starts.append(i)
    for st in starts:
        if len(s) % st != 0:
            continue
        target = s[:st]
        ok = True
        for i in range(st, len(s), st):
            cur = s[i:i+st]
            if cur != target:
                ok = False
                break
        if ok:
            return True
    return False


if __name__ == "__main__":
    main()

"""
What I learned?
- Splitting strings - "a,b,c".split(",")
- For-loop equivalents - for i in range(0, x, 3):
- Int → string - str(10)
- Substrings (slicing) - s[2:5]
- First half of a string - s[:len(s)//2]
- Lists + append - arr = []; arr.append(1)
- While loop - while i < 10: i += 1
- Passing functions - run(fn), fn()
"""
