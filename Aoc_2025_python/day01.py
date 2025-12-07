# 6027 ok
# 6038 too big

def main():
    filename = "_input.txt"
    with open(filename) as file:
        lines = [line.rstrip() for line in file]
    solve(lines)


def solve(lines: list):
    res1 = 0
    res2 = 0
    x = 50
    for line in lines:
        x, res1, res2 = solve_step(line, x, res1, res2)
    print("res1", res1, "res2", res2)


def solve_step(line: str, x: int, res1: int, res2: int):
    dx = (1 if line[0] == "R" else -1) * int(line[1:])
    x0 = x
    x = x + dx
    if x > 0:
        res2 += x // 100
        x = x % 100
    else:
        x = -x
        res2 += x // 100
        x = x % 100
        x = -x
        if x < 0:
            x += 100
        if x0 > 0:
            res2 += 1
    if x == 0:
        res1 += 1
    return x, res1, res2

if __name__ == "__main__":
    main()

"""
What I learned?
- Integer division is //
"""
