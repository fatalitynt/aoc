def sign(val):
    return 1 if val > 0 else (-1 if val < 0 else 0)


def draw_line(a, b, dct):
    dx = sign(b[0] - a[0])
    dy = sign(b[1] - a[1])
    dct[a] = "#"
    dct[b] = "#"
    while a != b:
        a = (a[0] + dx, a[1] + dy)
        dct[a] = "#"


def print_dct(dct, xx, h):
    for y in range(h):
        for x in range(500 - xx, 500 + xx):
            print(dct[(x, y)] if (x, y) in dct else ".", sep="", end="")
        print()


dxs = [0, -1, 1]


def goes_to_nowhere(p, dct, max_y):
    while True:
        upd = False
        for dx0 in dxs:
            p1 = (p[0] + dx0, p[1] + 1)
            if p1 in dct:
                continue
            p = p1
            # part 1 if
            # if p[1] > max_y:
            #     return True
            upd = True
            break
        if not upd:
            break
    # part 2 if
    if p in dct:
        return True
    dct[p] = "o"
    return False


def main():
    filename = "_input.txt"
    with open(filename) as file:
        lines = list(map(lambda z: z.rstrip(), file))
    dct = {}
    max_y = 0
    for line in lines:
        points = list(map(lambda xx: tuple(map(lambda y: int(y), xx.split(","))), line.split(" -> ")))
        max_y = max(max_y, max(map(lambda p: p[1], points)))
        for i in range(len(points) - 1):
            draw_line(points[i], points[i + 1], dct)

    # part 2 extra line
    draw_line((0, max_y + 2), (1000, max_y + 2), dct)

    cnt = 0
    while True:
        if goes_to_nowhere((500, 0), dct, max_y):
            break
        cnt += 1
    print(cnt)


if __name__ == '__main__':
    main()

'''
What I learned?
- nothing
'''
