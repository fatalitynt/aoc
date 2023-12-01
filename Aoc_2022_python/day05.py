def parse_line(line, stacks):
    for i in range(1, len(line), 4):
        if line[i] != " ":
            stacks[int(i / 4)].append(line[i])


def handle_move(move, stacks):
    parts = move.split()
    q = int(parts[1])
    s = int(parts[3]) - 1
    d = int(parts[5]) - 1

    # move for part - 1
    # for i in range(q):
    #     stacks[d].append(stacks[s].pop())

    # move for part - 2
    stacks[d].extend(stacks[s][-q:])
    stacks[s] = stacks[s][:-q]


def solve(lines):
    empty_idx = lines.index("")
    n = int(lines[empty_idx - 1].split()[-1].rstrip())
    stacks = []
    for i in range(n):
        stacks.append([])
    for i in range(empty_idx - 1):
        parse_line(lines[i], stacks)
    for s in stacks:
        s.reverse()
    for i in range(empty_idx + 1, len(lines)):
        handle_move(lines[i], stacks)
    res = ""
    for s in stacks:
        res += s.pop()
    return res


def main():
    filename = "_input.txt"

    with open(filename) as file:
        lines = list(map(lambda x: x.rstrip(), file))
        print(solve(lines))


if __name__ == '__main__':
    main()

'''
What I learned?
- index of is index(...)
- [] can be used as stack with .append() and .pop()
- last element is arr[-1]
- arr.extend() works as addRange()
- for i in range(start, exclusiveMax, delta) - c++ style for loop 
'''
