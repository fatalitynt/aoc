class Monkey:
    def __init__(self, lines):
        self.items = list(map(lambda x: int(x.strip()), lines[0].split(":")[1].split(",")))

        fn = lines[1].split("=")[1].strip()
        self.op = "+" if "+" in fn else "*"
        opr_str = fn.split(self.op)[1].strip()
        self.opr = -1 if opr_str == "old" else int(opr_str)

        self.test = int(lines[2].split()[-1])
        self.okay_direction = int(lines[3].split()[-1])
        self.fail_direction = int(lines[4].split()[-1])

        self.cnt = 0
        self.div = 0

    def pass_items(self, monkeys):
        for item in self.items:
            self.cnt += 1
            val1 = item
            val2 = val1 if self.opr == -1 else self.opr
            val = val1 + val2 if self.op == "+" else val1 * val2
            # part 1
            # val = val // 3
            # part 2
            val = val % self.div
            trg_idx = self.okay_direction if val % self.test == 0 else self.fail_direction
            monkeys[trg_idx].items.append(val)
        self.items = []


def main():
    filename = "_input.txt"
    with open(filename) as file:
        lines = list(map(lambda y: y.rstrip(), file))
        monkeys = []
        for i in range(len(lines)):
            line = lines[i]
            if line.startswith("Monkey"):
                monkeys.append(Monkey(lines[i + 1:i + 6]))

        div = 1
        for m in monkeys:
            div *= m.test

        for m in monkeys:
            m.div = div

        # part 1
        # n = 20
        # part 2
        n = 10000
        for i in range(n):
            for m in monkeys:
                m.pass_items(monkeys)

        counts = list(map(lambda x: x.cnt, monkeys))
        counts.sort()
        print(counts[-1] * counts[-2])


if __name__ == '__main__':
    main()

'''
What I learned?
- class creation/usage
'''