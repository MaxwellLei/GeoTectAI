# Packages required for the project
import pandas as pd
import numpy as np
import matplotlib.pyplot as plt
from sklearn.model_selection import train_test_split
from sklearn.preprocessing import StandardScaler, LabelEncoder
from sklearn.ensemble import RandomForestClassifier
from sklearn.tree import DecisionTreeClassifier
from sklearn.neighbors import KNeighborsClassifier
from sklearn.metrics import classification_report
from imblearn.over_sampling import SMOTE
from sklearn.metrics import confusion_matrix
import seaborn as sns
from sklearn.svm import SVC
from xgboost import XGBClassifier
import networkx as nx
from sklearn.model_selection import cross_val_score
from pandas.plotting import parallel_coordinates
from skl2onnx.common.data_types import FloatTensorType
from skl2onnx import convert_sklearn
from onnxmltools.convert import convert_xgboost
import shap
import joblib


# Various methods
def draw_fig_1():
    features_dropped['Tectonic setting'] = label_dropped
    for i, feature in enumerate(features_dropped.columns[:-1]):
        plt.figure(figsize=(10, 5))
        sns.violinplot(x='Tectonic setting', y=feature, data=features_dropped)
        plt.title(feature)
        plt.tight_layout()
        plt.show()


def draw_fig_2(label_series, feature_dataframe, column_ranges=None):
    if column_ranges is None:
        column_ranges = [(0, 9), (9, None)]
    unique_labels = label_series.unique()
    num_plots_per_row = 4
    num_rows = (len(unique_labels) + num_plots_per_row - 1) // num_plots_per_row

    for column_range in column_ranges:
        plt.figure(figsize=(15, 10))
        for i, tectonic_setting in enumerate(unique_labels):
            subset = feature_dataframe.loc[
                label_series == tectonic_setting, feature_dataframe.columns[column_range[0]:column_range[1]]]
            plt.subplot(num_rows, num_plots_per_row, i + 1)
            sns.violinplot(data=subset, orient='h')
            plt.title(tectonic_setting)
        plt.tight_layout()
        plt.show()


def draw_fig_3():
    plt.figure(figsize=(15, 10))
    sns.boxplot(data=features_filtered)
    plt.xticks(rotation=90)
    plt.title("Boxplot of features")
    plt.show()


def draw_fig_4():
    features_scaled_df = pd.DataFrame(features_scaled, columns=data.columns[7:])
    sns.set_style("dark")
    palette = sns.color_palette("mako", n_colors=len(features_scaled_df.columns))
    plt.figure(figsize=(15, 15))
    for i, column in enumerate(features_scaled_df.columns):
        plt.subplot(4, 6, i + 1)
        sns.histplot(features_scaled_df[column], kde=True, color=palette[i])
        plt.title(column)
    plt.tight_layout()
    plt.show()


def draw_fig_5():
    df_scaled = pd.DataFrame(features_scaled, columns=features_filtered.columns)
    df_scaled['Tectonic setting'] = label_filtered.values
    colormap = plt.cm.viridis
    plt.figure(figsize=(20, 10))
    parallel_coordinates(df_scaled, 'Tectonic setting', colormap=colormap, alpha=0.5)
    plt.xticks(rotation=90)
    plt.title('Parallel Coordinates Plot of Scaled Features')
    plt.show()


def draw_fig_6():
    fig, axes = plt.subplots(nrows=2, ncols=3, figsize=(20, 20))
    for ax, (name, matrix) in zip(axes.flatten(), confusion_matrices.items()):
        matrix_normalized = matrix.astype('float') / matrix.sum(axis=1)[:, np.newaxis]
        sns.heatmap(matrix_normalized, annot=True, ax=ax, cmap="Oranges", fmt=".2f")
        ax.set_title(f'{name}', fontsize=12)
        ax.tick_params(axis='both', which='major', labelsize=10)
    plt.tight_layout()
    plt.show()


def draw_fig_7():
    explainer_rf = shap.TreeExplainer(models['RandomForest'])
    explainer_xgb = shap.TreeExplainer(models['XGBoosting'])
    shap_values_rf = explainer_rf.shap_values(X_test)
    shap_values_xgb = explainer_xgb.shap_values(X_test)
    for i in range(7):
        plt.figure()
        shap.summary_plot(shap_values_rf[i], X_test, feature_names=features_filtered.columns, plot_type="dot",
                          show=False)
        plt.title(f"Random Forest SHAP Summary Plot for Class {i}")
        plt.show()
        plt.figure()
        shap.summary_plot(shap_values_xgb[i], X_test, feature_names=features_filtered.columns, plot_type="dot",
                          show=False)
        plt.title(f"XGBoost SHAP Summary Plot for Class {i}")
        plt.show()


def draw_fig_8():
    random_forest = models['RandomForest']
    xgboost = models['XGBoosting']
    rf_importances = random_forest.feature_importances_
    xgb_importances = xgboost.feature_importances_
    average_importance = (rf_importances + xgb_importances) / 2
    plt.figure(figsize=(10, 8))
    scatter = plt.scatter(rf_importances, xgb_importances, c=average_importance, cmap='viridis')
    colorbar = plt.colorbar(scatter)
    colorbar.set_label('Average Feature Importance')
    elements = ["SiO2(WT%)", "TiO2(WT%)", "Al2O3(WT%)", "CaO(WT%)", "MgO(WT%)", "MnO(WT%)", "K2O(WT%)", "Na2O(WT%)",
                "P2O5(WT%)", "La(PPM)", "Ce(PPM)", "Pr(PPM)", "Nd(PPM)", "Sm(PPM)", "Eu(PPM)", "Gd(PPM)", "Tb(PPM)",
                "Dy(PPM)", "Ho(PPM)", "Er(PPM)", "Tm(PPM)", "Yb(PPM)", "Lu(PPM)"]
    for i in range(len(rf_importances)):
        plt.text(rf_importances[i], xgb_importances[i], elements[i], fontsize=8)
    plt.plot([0, max(max(rf_importances), max(xgb_importances))], [0, max(max(rf_importances), max(xgb_importances))],
             color='red', linestyle='--')
    plt.xlabel('Random Forest Feature Importance')
    plt.ylabel('XGBoost Feature Importance')
    plt.title('Diagonal Volcano Plot Comparing Feature Importances')
    plt.show()


def draw_fig_10():
    random_forest = models['RandomForest']
    xgboost = models['XGBoosting']
    rf_importances = random_forest.feature_importances_
    xgb_importances = xgboost.feature_importances_

    def create_network_graph(model_name, importances):
        G = nx.Graph()
        for i, imp in enumerate(importances):
            G.add_edge(model_name, f'Feature {i}', weight=imp)

        pos = nx.spring_layout(G)
        edges = G.edges(data=True)
        nx.draw(G, pos, with_labels=True, node_color='skyblue', node_size=2000,
                edge_color=[data['weight'] for _, _, data in edges], width=2.0, edge_cmap=plt.cm.Blues, font_size=12)
        plt.title(f'Feature Importance Network Graph for {model_name}')
        plt.show()

    create_network_graph('RandomForest', rf_importances)
    create_network_graph('XGBoosting', xgb_importances)


def cv_5():
    for name, model in models.items():
        scores = cross_val_score(model, X_train_smote, y_train_smote, cv=10)
        avg_score = np.mean(scores)
        cv_scores[name] = avg_score
        if language == 'English':
            print(f"The average cross-validation score of the {name} model: {avg_score}")
        else:
            print(f"{name}模型的平均交叉验证分数: {avg_score}")


def create_model_onnx():
    joblib.dump(model, f'{name}_model.pkl')
    if name == 'XGBoosting':
        onnx_model = convert_xgboost(model, initial_types=initial_type)
    else:
        onnx_model = convert_sklearn(model, initial_types=initial_type)
    if onnx_model is not None:
        with open(f"{name}_model.onnx", "wb") as f:
            f.write(onnx_model.SerializeToString())


def execute_based_on_input(prompt_message, y_action, n_action, red_color=False):
    if red_color:
        user_input = input(RED + f"{prompt_message} (y/n))：" + RESET)
    else:
        user_input = input(f"{prompt_message} (y/n))：")
    while user_input.lower() not in ['y', 'n']:
        if language == 'English':
            print("Input is invalid, please enter y or n.")
            user_input = input("Please enter y or n：")
        else:
            print("输入无效，请输入y或n。")
            user_input = input("请输入y或n：")
    if user_input in ['y', 'Y']:
        y_action()
    elif user_input in ['n', 'N']:
        n_action()


def f_null():
    pass

# -------------------------------------------------------------------------
# -------⭐The only way to do great work is to love what you do⭐---------
# -------------------------------------------------------------------------


# Global parameters
plt.rcParams['font.sans-serif'] = ['Arial']  # Set Arial font
plt.rcParams['axes.unicode_minus'] = False  # Display negative signs correctly
RED = '\033[91m'
BLUE = '\033[94m'
GREEN = '\033[92m'
RESET = '\033[0m'

# language setting
language = input(GREEN + "Please select language（中文/English）: " + RESET)
while language not in ['中文', 'English']:
    print(GREEN + "The input is invalid, please enter 中文 or English." + RESET)
    language = input(GREEN + "Please select language（中文/English）: " + RESET)

# Load data
data = pd.read_excel('Database_7_AKA.xlsx')

# Select labels and features
label = data['Tectonic setting']
features = data.iloc[:, 7:]

# Preprocessing: Delete entry data containing null values
features_dropped = features.dropna()
label_dropped = label[features_dropped.index]

if language == 'English':
    execute_based_on_input("Whether to draw a boxplot of the corresponding category for each tectonic setting？",
                           lambda: draw_fig_2(label_dropped, features_dropped, [(0, 9), (9, None)]), f_null)
else:
    execute_based_on_input("是否绘制每个构造环境对应类别的箱线图？",
                           lambda: draw_fig_2(label_dropped, features_dropped, [(0, 9), (9, None)]), f_null)

# Handling Outliers: Use IQR
Q1 = features_dropped.quantile(0.25)
Q3 = features_dropped.quantile(0.75)
IQR = Q3 - Q1
mask = ((features_dropped >= (Q1 - 1.5 * IQR)) & (features_dropped <= (Q3 + 1.5 * IQR))).all(axis=1)
features_filtered = features_dropped.loc[mask]
label_filtered = label_dropped.loc[mask]

if language == 'English':
    execute_based_on_input("Whether or not to plot a boxplot with outliers removed？", draw_fig_3, f_null)
else:
    execute_based_on_input("是否绘制去除异常值后的箱线图？", draw_fig_3, f_null)

# standardized features
scaler = StandardScaler()
features_scaled = scaler.fit_transform(features_filtered)

if language == 'English':
    execute_based_on_input("Whether to draw a standardized histogram？", draw_fig_4, f_null)
    execute_based_on_input("Whether to draw a standardized parallel coordinate plot？", draw_fig_5, f_null)
else:
    execute_based_on_input("是否绘制标准化后的柱状图？", draw_fig_4, f_null)
    execute_based_on_input("是否绘制标准化后的平行坐标图？", draw_fig_5, f_null)

# use LabelEncoder
label_encoder = LabelEncoder()
label_encoded = label_encoder.fit_transform(label_filtered)

# Obtain the mapping relationship between the encoded category and the specific label
label_mapping = dict(zip(label_encoder.classes_, range(len(label_encoder.classes_))))

# Output mapping relationship
if language == 'English':
    print("Mapping relationship between encoded categories and specific labels：")
else:
    print("编码后的类别与具体标签的映射关系：")
for label, encoded_label in label_mapping.items():
    print(f"{encoded_label}: {label}")

# Split the data into training set and test set
X_train, X_test, y_train, y_test = train_test_split(features_scaled, label_encoded, test_size=0.2, random_state=42)

# Output the number of preprocessed data and the number of data in the training/test set
if language == 'English':
    print(f"The total number of preprocessed data: {len(features_scaled)}")
    print(f"Number of data items in the training set: {len(X_train)}")
    print(f"Number of data items in the test set: {len(X_test)}")
else:
    print(f"经过预处理的数据总条数: {len(features_scaled)}")
    print(f"训练集的数据条数: {len(X_train)}")
    print(f"测试集的数据条数: {len(X_test)}")

# Apply SMOTE to solve data imbalance problem
smote = SMOTE()
X_train_smote, y_train_smote = smote.fit_resample(X_train, y_train)

# Introduce and train the model
models = {
    'DecisionTree': DecisionTreeClassifier(),
    'KNeighbors': KNeighborsClassifier(),
    'RandomForest': RandomForestClassifier(),
    'XGBoosting': XGBClassifier(),
    'SVC': SVC()
}

# Dictionary used to store confusion matrix
confusion_matrices = {}
# Dictionary to store model performance reports
model_reports = {}
# Dictionary storing model cross-validation scores
cv_scores = {}

initial_type = [('float_input', FloatTensorType([None, 23]))]

if language == 'English':
    execute_based_on_input("Whether to perform 5-fold cross-validation？", cv_5, f_null)
else:
    execute_based_on_input("是否进行 5 折交叉验证？", cv_5, f_null)

for name, model in models.items():
    model.fit(X_train_smote, y_train_smote)
    predictions = model.predict(X_test)
    report = classification_report(y_test, predictions, output_dict=True)
    report_output = classification_report(y_test, predictions)
    model_reports[name] = report
    if language == 'English':
        print(f"The report of the {name} model:\n", report_output)
        execute_based_on_input(f"Whether to serialize the [{name}] model as an ONNX file？",
                               create_model_onnx, f_null, True)
    else:
        print(f"{name}模型的报告:\n", report_output)
        execute_based_on_input(f"是否序列化[{name}]模型为ONNX文件？", create_model_onnx, f_null, True)
    cm = confusion_matrix(y_test, predictions)
    confusion_matrices[name] = cm

if language == 'English':
    execute_based_on_input("Whether to draw the confusion matrix？", draw_fig_6, f_null)
    execute_based_on_input("Whether to draw the SHAP plot (it will take longer, please be patient)？",
                           draw_fig_7, f_null, True)
    execute_based_on_input("Whether to draw the RF and XGB feature weight network graph？", draw_fig_10, f_null)
    execute_based_on_input("Whether to draw the RF and XGB feature importance volcano plot？", draw_fig_8, f_null)
else:
    execute_based_on_input("是否绘制混淆矩阵？", draw_fig_6, f_null)
    execute_based_on_input("是否绘制SHAP图(时间会比较长，请耐心等待)？", draw_fig_7, f_null, True)
    execute_based_on_input("是否绘制RF和XGB特征权重网络图？", draw_fig_10, f_null)
    execute_based_on_input("是否绘制RF和XGB特征重要性火山图？", draw_fig_8, f_null)