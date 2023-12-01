win_dict = [-1, 3, 1, 2]
lose_dict = [-1, 2, 3, 1]
same_dict = [-1, 1, 2, 3]
dict_of_dict = [win_dict, same_dict, lose_dict]
moves = {"A": 1, "B": 2, "C": 3}  # 1-rock, 2-paper, 3-scissors
my_moves = {"X": 1, "Y": 2, "Z": 3}
round_result = {"X": 0, "Y": 1, "Z": 2}  # x-lose, y-draw, z-win


def round_score(raw_moves):  # part 1
    move = moves[raw_moves[0]]
    my_move = my_moves[raw_moves[1]]
    score = 0
    if move == my_move:
        score = 3
    elif win_dict[my_move] == move:
        score = 6
    return score + my_move


def real_round_score(raw_moves):  # part 2
    move = moves[raw_moves[0]]
    result = round_result[raw_moves[1]]
    return result * 3 + dict_of_dict[result][move]


def main():
    filename = "_input.txt"

    with open(filename) as file:
        print(sum(map(lambda x: real_round_score(x.rstrip().split()), file)))


if __name__ == '__main__':
    main()

'''
What I learned?
- dictionary usages
- string split function 
'''
