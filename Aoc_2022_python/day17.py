import time

wind_idx = 0
wind = []

block_idx = 0
blocks = [
    [[3, 4], [4, 4], [5, 4], [6, 4]],
    [[4, 4], [3, 5], [4, 5], [5, 5], [4, 6]],
    [[3, 4], [4, 4], [5, 4], [5, 5], [5, 6]],
    [[3, 4], [3, 5], [3, 6], [3, 7]],
    [[3, 4], [4, 4], [3, 5], [4, 5]],
]

left = 0
right = 8
dx = [0, 1, -1]
dy = [-1, 0, 0]

stones = {}
cols = {}
max_y = 0


def spawn_block():
    global max_y, block_idx
    block = blocks[block_idx]
    res = []
    for p in block:
        res.append([p[0], p[1] + max_y])
    block_idx = (block_idx + 1) % len(blocks)
    return res


def shift_block(block, i, mlp=1):
    for p in block:
        p[0] += dx[i] * mlp
        p[1] += dy[i] * mlp


def block_collided(block):
    global left, right, stones, max_y
    for p in block:
        if p[1] <= 0 or p[0] <= left or p[0] >= right or (p[0], p[1]) in stones:
            return True
    return False


def try_move(block):
    global left, right, stones, wind, wind_idx
    i = wind[wind_idx]
    wind_idx = (wind_idx + 1) % len(wind)
    shift_block(block, i)
    if block_collided(block):
        shift_block(block, i, -1)
    shift_block(block, 0)
    if block_collided(block):
        shift_block(block, 0, -1)
        return False
    return True


def fix_block(block, nb_block):
    global max_y, stones, cols, wind_idx
    for p in block:
        stones[(p[0], p[1])] = True
        max_y = max(max_y, p[1])
    if block_idx != 1 or block[0][0] != 2:
        return None
    if any((x, block[0][1]) in stones for x in [1, 6, 7]):
        return None
    if wind_idx in cols:
        cols[wind_idx][1] = nb_block
        cols[wind_idx][3] = max_y
        return cols[wind_idx]
    else:
        cols[wind_idx] = [nb_block, nb_block, max_y, max_y]
    return None


def solve(n):
    global max_y, stones
    i = n
    while i > 0:
        b = spawn_block()
        while try_move(b):
            continue
        shift = fix_block(b, i)
        if shift is not None:
            items_shift = shift[0] - shift[1]
            y_shift = shift[3] - shift[2]
            if items_shift > 0:
                max_y += y_shift * (i // items_shift)
                i %= items_shift
                for p in b:
                    stones[(p[0], max_y)] = True
        i -= 1
    print(max_y)


def main():
    global wind
    filename = "_input.txt"
    with open(filename) as file:
        lines = [line.rstrip() for line in file]
    wind = [1 if c == '>' else 2 for c in lines[0]]

    start_time = time.time()
    # part 1
    # solve(2022)
    # part 2
    solve(1000000000000)
    print("--- %s seconds ---" % (time.time() - start_time))


if __name__ == '__main__':
    main()

'''
What I learned?
- index can not be modified outside of loop as: for i in range(n)
'''
