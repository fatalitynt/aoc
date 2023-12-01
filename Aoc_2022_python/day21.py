import time


class Monkey:
    def __init__(self, line):
        parts = line.split(": ")
        self.name = parts[0]
        ops = parts[1].split(" ")
        self.l_key = None
        self.r_key = None
        self.op = None
        self.val = None
        if len(ops) == 1:
            self.val = int(ops[0])
        else:
            self.l_key = ops[0]
            self.r_key = ops[2]
            self.op = ops[1]

    def __repr__(self):
        return "(" + self.name + "):" + str(self.val if self.val is not None else "No")

    def left(self, m):
        return m[self.l_key]

    def right(self, m):
        return m[self.r_key]

    def get_val(self, m, custom=None):
        if self.val is not None:
            return self.val
        else:
            if self.l_key is None or self.r_key is None:
                return None
            left = self.left(m).get_val(m)
            right = self.right(m).get_val(m)
            if left is None or right is None:
                return None
            op = custom if custom is not None else self.op
            if op == "+":
                return left + right
            elif op == "-":
                return left - right
            elif op == "*":
                return left * right
            elif op == "/":
                return left // right
            elif op == "=":
                return 1 if left == right else 0

    def adjust_to_val(self, m, trg):
        if self.name == "humn":
            print("I should say", trg)
            return

        left = self.left(m)
        right = self.right(m)
        l_val = left.get_val(m)
        r_val = right.get_val(m)

        op = self.op

        if l_val is None:
            if op == "+":
                new_trg = trg - r_val
            elif op == "-":
                new_trg = trg + r_val
            elif op == "*":
                new_trg = trg // r_val
            else:
                new_trg = trg * r_val
            left.adjust_to_val(m, new_trg)
        else:
            if op == "+":
                new_trg = trg - l_val
            elif op == "-":
                new_trg = l_val - trg
            elif op == "*":
                new_trg = trg // l_val
            else:
                new_trg = trg // l_val
            right.adjust_to_val(m, new_trg)


def part1(m):
    return m["root"].get_val(m)


def part2(m):
    m["humn"].val = None
    root = m["root"]
    left = m[root.l_key]
    right = m[root.r_key]

    if left.get_val(m) is None:
        left.adjust_to_val(m, right.get_val(m))
    else:
        right.adjust_to_val(m, left.get_val(m))


def main():
    filename = "_input.txt"
    with open(filename) as file:
        lines = [line.rstrip() for line in file]
    m = {}
    for line in lines:
        monkey = Monkey(line)
        m[monkey.name] = monkey

    start_time = time.time()
    # part 1
    # print("part1:", part1(m))
    # part 2
    print("part2:", part2(m))
    print("--- %s seconds ---" % (time.time() - start_time))


if __name__ == '__main__':
    main()

'''
What I learned?
- nothing
'''
