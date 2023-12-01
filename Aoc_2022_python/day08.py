def check(h, w, dh, dw, h_max, w_max, a, added):
    top = a[h][w]
    added[h][w] = 1
    h += dh
    w += dw
    while h_max > h >= 0 and w_max > w >= 0:
        if a[h][w] > top:
            added[h][w] = 1
        top = max(top, a[h][w])
        h += dh
        w += dw


def solve1(h, w, a):
    added = []
    for r in range(0, h):
        added.append([0 for c in range(0, w)])

    for i in range(h):
        check(i, 0, 0, 1, h, w, a, added)
        check(i, w - 1, 0, -1, h, w, a, added)
    for j in range(w):
        check(0, j, 1, 0, h, w, a, added)
        check(h - 1, j, -1, 0, h, w, a, added)

    res = sum(map(lambda x: sum(x), added))
    print(res)


def count(h, w, dh, dw, h_max, w_max, a):
    res = 0
    my = a[h][w]
    h += dh
    w += dw

    while h_max > h >= 0 and w_max > w >= 0:
        if a[h][w] < my:
            res += 1
        if a[h][w] >= my:
            res += 1
            break
        h += dh
        w += dw

    return res


def solve2(h, w, a):
    max_score = 0
    dh = [-1, 0, 0, 1]  # u l r d
    dw = [0, -1, 1, 0]
    for i in range(h):
        for j in range(w):
            score = 1
            for x in range(4):
                cnt = count(i, j, dh[x], dw[x], h, w, a)
                score *= cnt
            max_score = max(max_score, score)
    print(max_score)


def main():
    filename = "_input.txt"
    with open(filename) as file:
        lines = list(map(lambda x: x.rstrip(), file))
        h = len(lines)
        w = len(lines[0])
        a = list(map(lambda x: list(map(lambda c: int(c), x)), lines))

        # part 1
        # solve1(h, w, a)
        # part 2
        solve2(h, w, a)


if __name__ == '__main__':
    main()

'''
What I learned?
- nothing
'''
