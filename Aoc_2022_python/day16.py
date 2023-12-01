from collections import deque
import time

dist = []
flow_rates = []
non_zero_flows = []
conn = []
full_flow = 0
names_map = {}
best_moves = {}
total_cases = 0
cntr = 0


def fill_dist(me):
    q = deque()
    q.append(me)
    dist[me][me] = 0
    while len(q) > 0:
        cur = q.popleft()
        for nxt in conn[cur]:
            if dist[me][nxt] != -1:
                continue
            dist[me][nxt] = dist[me][cur] + 1
            q.append(nxt)


def full_mask(idx_array):
    res = 0
    for i in idx_array:
        res |= 1 << i
    return res


def build_dp(n, t):
    dp = []
    for i in range(n):
        row = []
        for j in range(t + 1):
            row.append({})
        dp.append(row)
    return dp


def parse_input(lines):
    global dist, flow_rates, conn, full_flow, names_map, total_cases
    n = len(lines)
    for i in range(n):
        dist.append([-1] * n)
        conn.append([])
        flow_rates.append(0)

    for line in lines:
        left, right = line.split("; ")
        sp2 = left.split(" ")

        my_name = sp2[1]
        names = [x.split()[-1] for x in right.split(", ")]
        names.append(my_name)
        for name in names:
            if name not in names_map:
                names_map[name] = len(names_map)

        me = names_map[my_name]
        flow_rates[me] = int(sp2[-1].split("=")[-1])
        for other_id in [names_map[name] for name in names[:-1]]:
            conn[me].append(other_id)

    for i in range(n):
        fill_dist(i)
        if flow_rates[i] > 0:
            non_zero_flows.append(i)
    total_cases = pow(2, len(non_zero_flows))


def find_flow(me, minutes, state, full_state, nodes, cap, dp):
    global best_moves, names_map
    if state == full_state:
        return 0
    if state in dp[me][minutes]:
        return dp[me][minutes][state]
    res = 0
    for nxt in nodes:
        time_nxt_started = minutes + dist[me][nxt] + 1
        if state & (1 << nxt) != 0 or time_nxt_started >= cap:
            continue
        local_res = flow_rates[nxt] * (cap - time_nxt_started) + find_flow(nxt, time_nxt_started, state | (1 << nxt),
                                                                           full_state, nodes, cap, dp)
        if local_res > res:
            res = local_res

    dp[me][minutes][state] = res
    return res


def pick_and_solve(my_nodes, sup_nodes, all_nodes, pick_idx):
    global cntr, total_cases
    if pick_idx == len(all_nodes):
        # print("test", my_nodes, sup_nodes)
        minutes = 26
        start = names_map["AA"]

        my_dp = build_dp(len(flow_rates), minutes)
        my_res = find_flow(start, 0, 0, full_mask(my_nodes), my_nodes, minutes, my_dp)

        sup_dp = build_dp(len(flow_rates), minutes)
        sup_res = find_flow(start, 0, 0, full_mask(sup_nodes), sup_nodes, minutes, sup_dp)

        cntr += 1
        if cntr % 1000 == 0:
            print("checking", cntr, "/", total_cases)

        return my_res + sup_res

    my_nodes.append(all_nodes[pick_idx])
    res1 = pick_and_solve(my_nodes, sup_nodes, all_nodes, pick_idx + 1)
    my_nodes.pop()

    sup_nodes.append(all_nodes[pick_idx])
    res2 = pick_and_solve(my_nodes, sup_nodes, all_nodes, pick_idx + 1)
    sup_nodes.pop()
    return max(res1, res2)


def part1():
    minutes = 30
    dp = build_dp(len(flow_rates), minutes)
    res = find_flow(names_map["AA"], 0, 0, full_mask(non_zero_flows), non_zero_flows, minutes, dp)
    return res


def part2():
    return pick_and_solve([], [], non_zero_flows, 0)


def main():
    filename = "_input.txt"
    with open(filename) as file:
        lines = [line.rstrip() for line in file]
    parse_input(lines)
    print(names_map)
    print(total_cases)

    start_time = time.time()
    print(part2())
    print("--- %s seconds ---" % (time.time() - start_time))


if __name__ == '__main__':
    main()

'''
What I learned?
- nothing
'''
