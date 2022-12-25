# NOT MY SOLUTION FOR P2:
# USED: https://github.com/skbkontur/AoC2022/blob/main/src/day-19/index.js

import time
from collections import deque

names = [
    "ore",
    "clay",
    "obsidian"
]

t_max = 0
cache = {}
best_r = 0


def next_permutation(a):
    def swap(i, j):
        a[i], a[j] = a[j], a[i]

    def reverse_tail(i):
        j = len(a) - 1
        while i < j:
            swap(i, j)
            i += 1
            j -= 1

    if len(a) <= 1:
        return False

    i = len(a) - 1

    while True:
        i1 = i
        i -= 1
        if a[i] < a[i1]:
            i2 = len(a)
            while True:
                i2 -= 1
                if a[i] < a[i2]:
                    break
            swap(i, i2)
            reverse_tail(i1)
            return True

        if i == 0:
            reverse_tail(0)
            return False


def permutations(a):
    a = sorted(list(a))
    yield a
    while next_permutation(a):
        yield a


def build_bps(lines):
    inp = "".join(lines).replace("\n", "").replace(" ", "").replace("\t", "").split("Blueprint")
    res = []
    for i in inp[1:]:
        res.append(Blueprint(i))
    return res


class Blueprint:
    def __init__(self, input_str):
        parts = input_str.split(":")
        self.rec = []
        self.idx = int(parts[0])
        robots_line = parts[1].split(".")[:-1]
        for i in range(len(robots_line)):
            line = robots_line[i]
            costs = line.split("costs")[1]
            r = []
            for name in names[:i + 1]:
                r.append(self.get_q(costs, name))
            self.rec.append(r)
        self.mx = [
            max(map(lambda rc: rc[0], self.rec)),
            self.rec[2][1],
            self.rec[3][2]
        ]

    @staticmethod
    def get_q(line, name):
        parts = line.split(name)
        return int(parts[0].split("and")[-1]) if len(parts) > 1 else 0

    def prnt(self):
        print(self.idx, self.rec)

    def can_add_ore_r(self, ore):
        return self.rec[0][0] <= ore

    def can_add_clay_r(self, ore):
        return self.rec[1][0] <= ore

    def can_add_obs_r(self, ore, clay):
        return self.rec[2][0] <= ore and self.rec[2][1] <= clay

    def can_add_geo_r(self, ore, obs):
        return self.rec[3][0] <= ore and self.rec[3][2] <= obs


def get_max(ore, clay, obs, geo, ore_r, clay_r, obs_r, geo_r, bp: Blueprint, time_passed, time_avail):
    ore_r_added = False
    clay_r_added = False
    obs_r_added = False
    geo_r_added = False

    ans = geo

    for i in range(time_passed, time_avail):
        can_add_ore_r = bp.can_add_ore_r(ore)
        can_add_clay_r = bp.can_add_clay_r(ore)
        can_add_obs_r = bp.can_add_obs_r(ore, clay)
        can_add_geo_r = bp.can_add_geo_r(ore, obs)

        ore += ore_r
        clay += clay_r
        obs += obs_r
        geo += geo_r

        if can_add_ore_r and not ore_r_added and ore_r < bp.mx[0]:
            res = get_max(ore - bp.rec[0][0], clay, obs, geo,
                          ore_r + 1, clay_r, obs_r, geo_r,
                          bp, i + 1, time_avail)
            ans = max(ans, res)
            ore_r_added = True

        if can_add_clay_r and not clay_r_added and clay_r < bp.mx[1]:
            res = get_max(ore - bp.rec[1][0], clay, obs, geo,
                          ore_r, clay_r + 1, obs_r, geo_r,
                          bp, i + 1, time_avail)
            ans = max(ans, res)
            clay_r_added = True

        if can_add_obs_r and not obs_r_added and obs_r < bp.mx[2]:
            res = get_max(ore - bp.rec[2][0], clay - bp.rec[2][1], obs, geo,
                          ore_r, clay_r, obs_r + 1, geo_r,
                          bp, i + 1, time_avail)
            ans = max(ans, res)
            obs_r_added = True

        if can_add_geo_r and not geo_r_added:
            res = get_max(ore - bp.rec[3][0], clay, obs - bp.rec[3][2], geo,
                          ore_r, clay_r, obs_r, geo_r + 1,
                          bp, i + 1, time_avail)
            ans = max(ans, res)
            geo_r_added = True

        ans = max(ans, geo)

    return ans


def solve(bps, time_avail):
    res = 0
    for bp in bps:
        mx = get_max(0, 0, 0, 0,
                     1, 0, 0, 0,
                     bp, 0, time_avail)
        res += bp.idx * mx
        print("bp", bp.idx, "- max is", mx)
    return res


def main():
    global t_max
    filename = "_input.txt"
    with open(filename) as file:
        lines = [line for line in file]
    bps = build_bps(lines)

    start_time = time.time()
    print("part1 =", solve(bps, 24))
    print("part2 =", solve(bps[:3], 32))
    print("--- %s seconds ---" % (time.time() - start_time))


if __name__ == '__main__':
    main()

'''
What I learned?
- nothing
'''
