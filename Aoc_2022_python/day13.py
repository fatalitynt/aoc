import functools


def parse(line, i):
    res = []
    val = -1
    while True:
        if i == len(line):
            return [res, i]
        if line[i] == '[':
            rrr = parse(line, i + 1)
            res.append(rrr[0])
            i = rrr[1]
        elif line[i] == ']':
            if val >= 0:
                res.append(val)
            return [res, i + 1]
        elif line[i] == ',':
            if val >= 0:
                res.append(val)
            val = -1
            i += 1
        else:
            if val < 0:
                val = 0
            val *= 10
            val += int(line[i])
            i += 1


def sign(a):
    return 1 if a > 0 else (-1 if a < 0 else 0)


def wrap(a):
    return [a] if type(a) == int else a


def compare(l, r):
    i = 0
    while True:
        if i == len(l) and i == len(r):
            return 0
        if i == len(l):
            return 1
        if i == len(r):
            return -1
        cmp = sign(r[i] - l[i]) if type(l[i]) == int and type(r[i]) == int else compare(wrap(l[i]), wrap(r[i]))
        if cmp != 0:
            return cmp
        i += 1


def part1(lines):
    n = len(lines) // 3
    res = 0
    for i in range(n):
        left = parse(lines[i * 3], 1)[0]
        right = parse(lines[i * 3 + 1], 1)[0]
        if compare(left, right) > 0:
            res += (i + 1)
    print(res)


def part2(lines):
    items = list(map(lambda y: parse(y, 1)[0], filter(lambda x: x.strip() != "", lines)))
    items.append([[2]])
    items.append([[6]])
    items.sort(key=functools.cmp_to_key(compare), reverse=True)
    i1 = items.index([[2]]) + 1
    i2 = items.index([[6]]) + 1
    print(i1 * i2)


def main():
    filename = "_input.txt"
    with open(filename) as file:
        lines = list(map(lambda y: y.rstrip(), file))
    # part 1
    part1(lines)
    # part 2
    part2(lines)


if __name__ == '__main__':
    main()

'''
What I learned?
- import functools, then arr.sort(key=functools.cmp_to_key(compare))
'''
