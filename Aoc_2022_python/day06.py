def unique(a):
    for i in range(0, len(a)):
        for j in range(i + 1, len(a)):
            if a[i] == a[j]:
                return False
    return True


def solve(s, offset):
    n = len(s)
    for i in range(n - offset):
        if unique(s[i:i + offset]):
            return i + offset
    return -1


def main():
    filename = "_input.txt"

    with open(filename) as file:
        lines = list(map(lambda x: x.rstrip(), file))
        # part 1
        # print(solve(lines[0], 4))
        # part 2
        print(solve(lines[0], 14))


if __name__ == '__main__':
    main()

'''
What I learned?
- nothing
'''
