data_matrix = readcell("Test.csv");
data_matrix = data_matrix(2:length(data_matrix),:);
%split matrix to different interventions


%{
extract_base = data_matrix(:,5) == "Base";
Base = data_matrix(extract_base,:);
Base = cell2mat(Base(:,1:4));
% plot 
X = 1:length(Base);
for i=1:4
    Y = Base(:,i);
    figure
    plot(X,Y);
    title('Base')
    if i == 1
        xlabel('Seconds')
        ylabel('Height(m)')
    elseif i == 2
        xlabel('Seconds')
        ylabel('Angle(degree)')
    elseif i == 3
        xlabel('Seconds')
        ylabel('Posture state')
    elseif i == 4
        xlabel('Seconds')
        ylabel('Intervention Triggered')
    end
end
%}

extract_icon = data_matrix(:,5) == "Icon";
Icon = data_matrix(extract_icon,:);
Icon = cell2mat(Icon(:,1:4));
% plot 
X = 1:length(Icon);
for i=1:4
    Y = Icon(:,i);
    figure
    plot(X,Y);
    title('Icon')
    if i == 1
        xlabel('Seconds')
        ylabel('Height(m)')
    elseif i == 2
        xlabel('Seconds')
        ylabel('Angle(degree)')
    elseif i == 3
        xlabel('Seconds')
        ylabel('Posture state')
    elseif i == 4
        xlabel('Seconds')
        ylabel('Intervention Triggered')
    end
end

%{
extract_dot = data_matrix(:,5) == "Dot";
Dot = data_matrix(extract_dot,:);
Dot = cell2mat(Dot(:,1:4));
% plot 
X = 1:length(Dot);
for i=1:4
    Y = Dot(:,i);
    figure
    plot(X,Y);
    title('Dot')
    if i == 1
        xlabel('Seconds')
        ylabel('Height(m)')
    elseif i == 2
        xlabel('Seconds')
        ylabel('Angle(degree)')
    elseif i == 3
        xlabel('Seconds')
        ylabel('Posture state')
    elseif i == 4
        xlabel('Seconds')
        ylabel('Intervention Triggered')
    end
end

extract_environment = data_matrix(:,5) == "Environment";
Environment = data_matrix(extract_environment,:);
Environment = cell2mat(Environment(:,1:4));
% plot 
X = 1:length(Environment);
for i=1:4
    Y = Environment(:,i);
    figure
    plot(X,Y);
    title('Environment')
    if i == 1
        xlabel('Seconds')
        ylabel('Height(m)')
    elseif i == 2
        xlabel('Seconds')
        ylabel('Angle(degree)')
    elseif i == 3
        xlabel('Seconds')
        ylabel('Posture state')
    elseif i == 4
        xlabel('Seconds')
        ylabel('Intervention Triggered')
    end
end
%}
