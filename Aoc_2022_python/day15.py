def parse_line(s):
    return list(map(lambda h: tuple(map(lambda v: int(v.split("=")[1]), h.split("at ")[1].split(", "))), s.split(": ")))


def get_sector(points, y):
    sen, bac = points
    dist = abs(sen[0] - bac[0]) + abs(sen[1] - bac[1])
    d_y = abs(y - sen[1])
    d_dist = dist - d_y
    if d_dist < 0:
        return None
    return sen[0] - d_dist, sen[0] + d_dist


def set_sector(sector, y_map):
    if sector is None:
        return
    for i in range(sector[0], sector[1] + 1):
        if i in y_map:
            continue
        y_map[i] = 0


def try_merge(sectors):
    end = None
    for sector in sectors:
        if end is None:
            end = sector[1]
        else:
            if sector[0] > end + 1:
                return end + 1
            else:
                end = max(end, sector[1])
    return None


def part1(all_points):
    y = 2000000
    y_map = {}
    for points in all_points:
        sector = get_sector(points, y)
        set_sector(sector, y_map)
        for p in points:
            if p[1] == y:
                y_map[p[0]] = 1
    print(len(list(filter(lambda k: y_map[k] == 0, y_map.keys()))))


def part2(all_points):
    max_x_y = 4000000
    for i in range(1, max_x_y):
        sectors = sorted(list(filter(lambda sct: sct is not None, map(lambda pts: get_sector(pts, i), all_points))))
        hole = try_merge(sectors)
        if hole is not None:
            print(hole, i, hole * max_x_y + i)
            break


def main():
    filename = "_input.txt"
    with open(filename) as file:
        lines = [line.rstrip() for line in file]
    all_points = list(map(parse_line, lines))

    # part1(all_points)
    part2(all_points)


if __name__ == '__main__':
    main()

'''
What I learned?
- nothing
'''
