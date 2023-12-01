import time

first_idx = 0
d_key = 811589153


class Item:
    def __init__(self, val, idx, mlp):
        self.val = val * mlp
        self.idx = idx
        self.next = None
        self.prev = None

    def __repr__(self):
        return "(" + str(self.val) + ", i:" + str(self.idx) + ")"

    def shift_right(self):
        connect(self.prev, self.next)
        one_after = self.next.next
        connect(self.next, self)
        connect(self, one_after)

    def shift_left(self):
        connect(self.prev, self.next)
        one_before = self.prev.prev
        connect(self, self.prev)
        connect(one_before, self)

    def shift(self):
        global first_idx
        if first_idx == self.idx:
            first_idx = self.next.idx
        self.shift_right() if self.val > 0 else self.shift_left()


def connect(a: Item, b: Item):
    a.next = b
    b.prev = a


def find_by_val(a: Item, i):
    while a.val != i:
        a = a.next
    return a


def find(a: Item, i):
    while a.idx != i:
        a = a.next
    return a


def get(a: Item, i):
    for j in range(i):
        a = a.next
    return a


def solve(lines, mlp, nb_mix):
    first = None
    last = None
    n = len(lines)
    for i in range(n):
        item = Item(int(lines[i]), i, mlp)
        if last is None:
            first = item
            last = item
        else:
            connect(last, item)
            last = item
    connect(last, first)

    for y in range(nb_mix):
        print("mixing iteration", y)
        for i in range(n):
            item = find(first, i)
            dist = abs(item.val) % (n - 1)
            for j in range(dist):
                item.shift()

    p = find_by_val(first, 0)
    res = 0
    for x in [1000, 2000, 3000]:
        res += get(p, x % n).val

    return res


def part1(lines):
    return solve(lines, 1, 1)


def part2(lines):
    return solve(lines, d_key, 10)


def main():
    filename = "_input.txt"
    with open(filename) as file:
        lines = [line.rstrip() for line in file]

    start_time = time.time()
    # part 1
    # print("part1:", part1(lines))
    # part 2
    print("part2:", part2(lines))
    print("--- %s seconds ---" % (time.time() - start_time))


if __name__ == '__main__':
    main()

'''
What I learned?
- nothing
'''
